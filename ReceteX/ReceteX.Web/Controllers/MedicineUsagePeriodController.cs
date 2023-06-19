using Microsoft.AspNetCore.Mvc;
using ReceteX.Repository.Shared.Abstract;

namespace ReceteX.Web.Controllers
{
	public class MedicineUsagePeriodController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public MedicineUsagePeriodController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult GetAll()
		{
			return Json(_unitOfWork.MedicineUsagePeriods.GetAll().OrderBy(m => m.RemoteId));
		}
	}
}