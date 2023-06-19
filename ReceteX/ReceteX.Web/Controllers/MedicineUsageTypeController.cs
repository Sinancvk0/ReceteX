using Microsoft.AspNetCore.Mvc;
using ReceteX.Repository.Shared.Abstract;

namespace ReceteX.Web.Controllers
{
	public class MedicineUsageTypeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public MedicineUsageTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult GetAll()
		{
			return Json(_unitOfWork.MedicineUsageTypes.GetAll().OrderBy(m => m.RemoteId));
		}
	}
}