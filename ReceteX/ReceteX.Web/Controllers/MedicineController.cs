using Microsoft.AspNetCore.Mvc;

namespace ReceteX.Web.Controllers
{
	public class MedicineController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
