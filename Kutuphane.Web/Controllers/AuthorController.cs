using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Abstract;
using Kutuphane.Repository.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Kutuphane.Web.Controllers
{
	public class AuthorController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public AuthorController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetAll()
		{
			List<Author> authors = unitOfWork.Authors.GetAll().ToList<Author>();
			return Json(new { data = authors });
		}


		[HttpPost]
		public IActionResult Create(Author author)
		{
			if (author.Name != null)
			{
				unitOfWork.Authors.Add(author);
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
			//Author author = _db.Authors.FirstOrDefault(c => c.Id == id);
			Author author = unitOfWork.Authors.GetFirstOrDefault(c => c.Id == id);

			if (author != null)
			{
				unitOfWork.Authors.Remove(author);
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
			//Author author = _db.Authors.FirstOrDefault(c => c.Id == id);
			Author author = unitOfWork.Authors.GetFirstOrDefault(c => c.Id == id);

			return Ok(author);
		}

		[HttpPost]
		public IActionResult Update(Author author)
		{
			if (author.Name != null)
			{
				Author asil = unitOfWork.Authors.GetFirstOrDefault(c => c.Id == author.Id);

				asil.Description = author.Description;
				asil.Name = author.Name;
				//_db.Authors.Update(asil);
				unitOfWork.Authors.Update(asil);
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