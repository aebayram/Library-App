using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Shared.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Kutuphane.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public UserController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		static string RandomStringGenerator(int length)
		{
			//Bu metodun amacı, girilen uzunluk değeri kadar uzun bir rastgele string oluşturmaktır.
			//stringi oluştururken, Büyükharf, Küçükarf ve Rakam kullanılacaktır.
			//Ancak içerisinde kaç büyükharf kaç küçük harf ve kaç rakam olacağı RASTGELE olarak belirlenecektir.
			//Ama, en az bir büyük harf, en az bir küçük harf ve en az bir rakam olması, garanti edilecektir.
			Random rnd = new Random();
			string karakterler = "ag4567hijklmnoprstuvyzx1238bcdef90QWERTYUIOPASDFGHJKLZXCVBNM";
			string randomString = "";
			if (length >= 3)
			{


				for (int i = 0; i < length - 3; i++)
					randomString += karakterler[rnd.Next(0, karakterler.Length)];

				//bir tane küçük harf enjekte edelim.
				randomString = randomString.Insert(rnd.Next(0, randomString.Length), Convert.ToChar(rnd.Next(97, 123)).ToString());

				//bir tane de büyük harf enjekte edelim

				randomString = randomString.Insert(rnd.Next(0, randomString.Length), Convert.ToChar(rnd.Next(65, 91)).ToString());

				//bir tane de rakam enjekte edelim

				randomString = randomString.Insert(rnd.Next(0, randomString.Length), rnd.Next(0, 10).ToString());

			}

			return randomString;
		}


		[Authorize(Roles = "Admin,User")]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Create(AppUser appuser)
		{
			if (appuser.Name != null)
			{
				AppUser asil = unitOfWork.Users.GetFirstOrDefault(c => c.Id == appuser.Id);

				appuser.Password = RandomStringGenerator(6);

				MD5 md5 = MD5.Create();
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(appuser.Password);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				string md5string = Convert.ToHexString(hashBytes); // .NET 5 +
				appuser.Password = md5string;
				Console.WriteLine("Password = " + appuser.Password);
				Console.WriteLine("MD5 Password = " + md5string);


				unitOfWork.Users.Add(appuser);
				unitOfWork.Save();
				return Ok("aferin çalıştı");
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpDelete]
		public IActionResult Delete(Guid id)
		{
			AppUser appuser = unitOfWork.Users.GetFirstOrDefault(c => c.Id == id);
			if (appuser != null)
			{
				unitOfWork.Users.Remove(appuser);
				unitOfWork.Save();
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public IActionResult GetById(Guid id)
		{
			AppUser appuser = unitOfWork.Users.GetFirstOrDefault(c => c.Id == id);

			return Ok(appuser);
		}
		[HttpPost]
		public IActionResult Update(AppUser appuser)
		{
			if (appuser.Name != null)
			{
				AppUser asil = unitOfWork.Users.GetFirstOrDefault(c => c.Id == appuser.Id);

				asil.UserName = appuser.UserName;
				asil.Name = appuser.Name;
				asil.isAdmin = appuser.isAdmin;
				unitOfWork.Users.Update(asil);
				unitOfWork.Save();
				return Ok();
			}
			else
			{
				return BadRequest();
			}


		}
		public IActionResult GetAll()
		{
			// Tüm User ları DataTables'a uygun şekilde JSON gönderecek kodu yazalım
			// Views/User altında index.cshtml oluşturalım . Bu sayfada datatable's ile sistemdeki tüm kullanıcıları gösterelim. [id],[UserName],[Name]
			// Sayfanın herhangi bir yerine Kullanıcı Ekle butonu koyalım, Modal açtıralım ve orada eklenecek olan kullanıcının bilgilerini ve Role'unu seçtirerek kayıt işlemini tamamlayalım. Ve datatable'sı Reload edelim.
			//Not: Kullanıcı ekleme sırasında, Formda Şifre istemeyelim. Şifre sistem tarafından rastgele otomatik oluşup veritabanına öyle kaydolsun.
			//DataTables'ın işlemler diye bir sütunu olsun orada Düzenle ve Sil fonksiyonları olsun.


			List<AppUser> appusers = unitOfWork.Users.GetAll().ToList<AppUser>();
			return Json(new { data = appusers });
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(AppUser user)
		{

			MD5 md5 = MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(user.Password);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			string md5string = Convert.ToHexString(hashBytes); // .NET 5 +
			AppUser appuser = unitOfWork.Users.GetFirstOrDefault(u => u.UserName == user.UserName && (u.Password== md5string || u.Password == user.Password));
			if (appuser != null)
			{
				List<Claim> claims = new List<Claim>();
				claims.Add(new Claim(ClaimTypes.Name, appuser.UserName));
				claims.Add(new Claim(ClaimTypes.GivenName, appuser.Name));
				claims.Add(new Claim(ClaimTypes.NameIdentifier, appuser.Id.ToString()));

				if (appuser.isAdmin)
				{
					claims.Add(new Claim(ClaimTypes.Role, "Admin"));
				}
				else
				{
					claims.Add(new Claim(ClaimTypes.Role, "User"));
				}

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

				return RedirectToAction("Index", "Home");
			}

			return View();

		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}
	}
}