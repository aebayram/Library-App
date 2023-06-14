using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Abstract;
using Kutuphane.Repository.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kutuphane.Web.Controllers
{
	public class BookController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public BookController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetAll()
		{
			//List<Book> list = _db.Books.Include(p=>p.Publishers).Include(b => b.Category).Include(x => x.Authors).ToList<Book>();

			//Alltaki gibi yapabiliriz ama BookRepositorty içinde de uyapabiliyoruz onu kullandık.
			//List<Book> list = _bookRepo.GetAll().Select(x => new Book
			//{
			//    Id=x.Id,
			//    Name=x.Name,
			//    Publishers=x.Publishers,
			//    Authors=x.Authors,
			//    Description=x.Description,
			//    Price=x.Price,
			//    TotalPages=x.TotalPages
			//}).ToList<Book>();

			return Json(new { data = unitOfWork.Books.GetAll().ToList<Book>() });
		}


		[HttpPost]
		public IActionResult RemoveAuthorFromBook(Guid bookId, Guid authorId)
		{
			//Bu şekilde ya da aşağıdaki gibi
			//Book book = (Book)_bookRepo.GetAll(b => b.Id == bookId).Select(b => new Book
			//{
			//    Id = b.Id,
			//    Authors = b.Authors
			//});

			Book book = unitOfWork.Books.GetAll(b => b.Id == bookId).First();
			// silinecekYazar = _db.Authors.FirstOrDefault(b => b.Id == authorId);
			Author silinecekYazar = book.Authors.FirstOrDefault(a => a.Id==authorId);

			book.Authors.Remove(silinecekYazar);
			unitOfWork.Books.Update(book);
			unitOfWork.Save();
			return Ok("");

		}


		public IActionResult removePublisherFromBook(Guid bookId, Guid publisherId)
		{
			Book book = unitOfWork.Books.GetAll(b => b.Id == bookId).First();
			// silinecekYazar = _db.Publishers.FirstOrDefault(b => b.Id == publisherId);
			Publisher silinecekYazar = book.Publishers.FirstOrDefault(p => p.Id == publisherId);

			book.Publishers.Remove(silinecekYazar);
			unitOfWork.Books.Update(book);
			unitOfWork.Save();
			return Ok("");

		}

		[HttpPost]
		public IActionResult Create(Book book, List<Author> authors, List<Publisher> publishers)
		{
			if (book.Name != null)
			{
				unitOfWork.Books.Add(book);
				//_db.SaveChanges();

				Book eklenenKitapVeYayınevi = unitOfWork.Books.GetFirstOrDefault(b => b.Id==book.Id);
				eklenenKitapVeYayınevi.Authors = authors;
				eklenenKitapVeYayınevi.Publishers = publishers;

				unitOfWork.Books.Update(eklenenKitapVeYayınevi);
				unitOfWork.Save();


				return Ok("");
			}
			else
			{
				return BadRequest();
			}

		}
		[HttpDelete]
		public IActionResult Delete(Guid id)
		{
			Book book = unitOfWork.Books.GetFirstOrDefault(c => c.Id == id);
			if (book != null)
			{
				unitOfWork.Books.Remove(book);
				unitOfWork.Save();
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public IActionResult AddAuthorToBook(Guid bookId, List<Guid> authors)
		{
			Book kitap = unitOfWork.Books.GetFirstOrDefault(b => b.Id==bookId);

			//tek linq sorgusuyla, dongü kurmaya gerek kalmadan, authos isimli Guid listemize bir select sorgusu atıyoruz ve bu sorgudan gelen her sonuca göre _db.Authors.FirstOrDefault diyerek veritabanında ilgili author nesnelerini çekiyoruz.
			List<Author> yazarlar = authors.Select(authorId => unitOfWork.Authors.GetFirstOrDefault(a => a.Id == authorId)).ToList();

			//List<Author> yazarlar = new List<Author>();

			//foreach (Guid authorId in authors)
			//{
			//    yazarlar.Add(_db.Authors.FirstOrDefault(a => a.Id == authorId));
			//}

			kitap.Authors= yazarlar;
			unitOfWork.Books.Update(kitap);
			unitOfWork.Save();
			return Ok();
		}

		[HttpPost]
		public IActionResult AddPublisherToBook(Guid bookId, List<Publisher> publishers)
		{
			Book yayınevi = unitOfWork.Books.GetFirstOrDefault(b => b.Id == bookId);
			yayınevi.Publishers = publishers;
			unitOfWork.Books.Update(yayınevi);
			unitOfWork.Save();
			return Ok();
		}

		[HttpPost]
		public IActionResult GetById(Guid id)
		{
			Book book = unitOfWork.Books.GetFirstOrDefault(c => c.Id == id);

			return Ok(book);
		}

		[HttpPost]
		public IActionResult Update(Book book)
		{
			if (book.Name != null)
			{
				Book asil = unitOfWork.Books.GetFirstOrDefault(c => c.Id == book.Id);

				asil.Name = book.Name;
				asil.Description = book.Description;
				asil.Isbn = book.Isbn;
				asil.Price = book.Price;
				asil.TotalPages= book.TotalPages;
				asil.DateModified = DateTime.Now;
				unitOfWork.Books.Update(asil);
				unitOfWork.Save();
				return Ok();
			}
			else
			{
				return BadRequest();
			}


		}

	}
}