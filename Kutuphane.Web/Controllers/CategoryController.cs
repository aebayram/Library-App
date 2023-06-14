using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.Web.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public CategoryController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetAll()
		{
			List<Category> categories = unitOfWork.Categories.GetAll().ToList<Category>();
			return Json(new { data = categories });
		}


		[HttpPost]
		public IActionResult Create(Category category)
		{
			if (category.Name != null)
			{
				unitOfWork.Categories.Add(category);
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
			Category category = unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
			if (category != null)
			{
				unitOfWork.Categories.Remove(category);
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
			Category category = unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);

			return Ok(category);
		}

		[HttpPost]
		public IActionResult Update(Category category)
		{
			if (category.Name != null)
			{
				Category asil = unitOfWork.Categories.GetFirstOrDefault(c => c.Id == category.Id);

				asil.Description = category.Description;
				asil.Name = category.Name;
				unitOfWork.Categories.Update(asil);
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