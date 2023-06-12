using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Data;
using System.Runtime.CompilerServices;

namespace ReceteX.Web.Controllers
{
	public class CustomerController : Controller
	{

		private readonly IUnitOfWork unitOfWork;
		public CustomerController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		[Authorize(Roles = "Admin")]

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetAll()
		{
			return Json(new { data = unitOfWork.Customers.GetAllWithUserCount() });

		}
		[HttpPost]
		public IActionResult Delete(Guid id)
		{

			unitOfWork.Customers.Remove(id);
			unitOfWork.Save();
			return Ok();

		}

		[HttpPost]
		public IActionResult GetById(Guid id)
		{
			return Json(unitOfWork.Customers.GetById(id));
			
		}
		[HttpPost]
		public IActionResult Create(Customer customer)
		{
			unitOfWork.Customers.Add(customer);
			unitOfWork.Save();
			return Ok();

		}

		[HttpPost]
		public IActionResult Update(Customer customer)
		{
			Customer asil = unitOfWork.Customers.GetFirstOrDefault(c => c.Id == customer.Id);
            asil.Name = customer.Name;
			unitOfWork.Customers.Update(asil);
			unitOfWork.Save();
			return Ok();

		}
	}
}
