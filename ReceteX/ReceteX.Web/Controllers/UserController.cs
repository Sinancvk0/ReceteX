using Microsoft.AspNetCore.Mvc;
using ReceteX.Repository.Shared.Abstract;

namespace ReceteX.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login()
		{
			return View();

		}
		[HttpPost]

	}
}
