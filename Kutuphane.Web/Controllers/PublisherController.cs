using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Abstract;
using Kutuphane.Repository.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Kutuphane.Web.Controllers
{
	public class PublisherController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public PublisherController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetAll()
		{
			List<Publisher> publishers = unitOfWork.Publishers.GetAll().ToList<Publisher>();
			return Json(new { data = publishers });
		}


		[HttpPost]
		public IActionResult Create(Publisher publisher)
		{
			if (publisher.Name != null)
			{
				unitOfWork.Publishers.Add(publisher);
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
			Publisher publisher = unitOfWork.Publishers.GetFirstOrDefault(c => c.Id == id);
			if (publisher != null)
			{
				unitOfWork.Publishers.Remove(publisher);
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
			Publisher publisher = unitOfWork.Publishers.GetFirstOrDefault(c => c.Id == id);

			return Ok(publisher);
		}

		[HttpPost]
		public IActionResult Update(Publisher publisher)
		{
			if (publisher.Name != null)
			{
				Publisher asil = unitOfWork.Publishers.GetFirstOrDefault(c => c.Id == publisher.Id);

				asil.Description = publisher.Description;
				asil.Name = publisher.Name;
				unitOfWork.Publishers.Update(asil);
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