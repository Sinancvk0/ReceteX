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
			return Json(new { data = unitOfWork.Customers.GetAll() });

		}

		[HttpPost]
		public IActionResult GetByID (Guid guid)
		{
			Customer cst = unitOfWork.Customers.GetFirstOrDefault(c => c.Id == guid);

			return Json(cst);
		}
		[HttpPost]
		public  IActionResult Create(Customer customer)
		{
			unitOfWork.Customers.Add(customer);
			unitOfWork.Save();
			return Ok();

		}

		[HttpPost]
		public IActionResult Update ( Customer customer)
		{
			unitOfWork.Customers.Update(customer);
			unitOfWork.Save();
			return Ok();

		}
	}
}
