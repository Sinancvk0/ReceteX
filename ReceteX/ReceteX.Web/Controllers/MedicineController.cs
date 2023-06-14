using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Xml;

namespace ReceteX.Web.Controllers
{
	public class MedicineController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public MedicineController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task ParseAndSaveFromXml(string xmlContent)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlContent);
			XmlNodeList medicines = xmlDoc.SelectNodes("ilaclar/ilac");
			
			foreach (XmlNode medicine in medicines )
			{
				Medicine med = new Medicine();
				med.Name = medicine.SelectSingleNode("ad").InnerText;
				med.Barcode = medicine.SelectSingleNode("barkod").InnerText;

				unitOfWork.Medicines.Add(med);
			}
			unitOfWork.Save();

		}
		public IActionResult UpdateMedicineList()
		{

			return RedirectToAction();
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
