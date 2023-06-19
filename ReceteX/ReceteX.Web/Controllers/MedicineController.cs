using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Utility;
using System.Xml;

namespace ReceteX.Web.Controllers
{
	public class MedicineController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly XmlRetriever xmlRetriever;

		public MedicineController(IUnitOfWork unitOfWork, XmlRetriever xmlRetriever)
		{
			this.unitOfWork = unitOfWork;
			this.xmlRetriever = xmlRetriever;
		}

		public async Task ParseAndSaveFromXml(string xmlContent)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlContent);
			XmlNodeList medicinesFromXml = xmlDoc.SelectNodes("/ilaclar/ilac");
			//datatabse'deki tüm aktif nesneler
			IQueryable<Medicine> medicinesFromDb = unitOfWork.Medicines.GetAll().AsNoTracking().OrderBy(m => m.Name).ToList().AsQueryable<Medicine>();

			//database'deki tüm silinmiş nesneler
			IQueryable<Medicine> deletedMedicinesFromDb = unitOfWork.Medicines.GetAllDeleted().AsNoTracking().OrderBy(m => m.Name).ToList().AsQueryable<Medicine>();

			//Yeni kayıtları aktaran döngümüz.
			foreach (XmlNode medicine in medicinesFromXml)
			{
				string barcodeFromXml = medicine.SelectSingleNode("barkod").InnerText;

				if (!medicinesFromDb.Any(m => m.Barcode == barcodeFromXml))
				{
					Medicine med = new Medicine();
					med.Name = medicine.SelectSingleNode("ad").InnerText;
					med.Barcode = barcodeFromXml;
					unitOfWork.Medicines.Add(med);
				}
				else
				{
					Medicine medSilinmis = deletedMedicinesFromDb.FirstOrDefault(m => m.Barcode == barcodeFromXml);
					if (medSilinmis != null)
					{
						medSilinmis.isDeleted = false;
						unitOfWork.Medicines.Update(medSilinmis);
					}
				}
			}

			unitOfWork.Save();

			//Kaynaktan silinmiş olan ilaçları databasede isdelete=true yapan döngümüz
			IEnumerable<XmlNode> medicinesFromXmlEnumarable = xmlDoc.SelectNodes("/ilaclar/ilac").Cast<XmlNode>();

			foreach (Medicine ilac in medicinesFromDb)
			{
				if (!medicinesFromXmlEnumarable.Any(x => x.SelectSingleNode("barkod").InnerText == ilac.Barcode))
				{
					ilac.isDeleted = true;
					unitOfWork.Medicines.Update(ilac);
				}

			}


			unitOfWork.Save();

		}

		public async Task<IActionResult> UpdateMedicinesList()
		{
			string content = await xmlRetriever.GetXmlContent("https://www.ibys.com.tr/exe/ilaclar.xml");
			await ParseAndSaveFromXml(content);



			return RedirectToAction("Index");
		}


		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult GetAll()
		{
			return Json(new { data = unitOfWork.Medicines.GetAll() });
		}


		public JsonResult SearchMedicine(string searchTerm)
		{
			var medicines = unitOfWork.Medicines.GetAll(d => d.Name.ToLower().Contains(searchTerm.ToLower())).Select(d => new { d.Id, Name = d.Name }).ToList();

			return Json(medicines);
		}
	}
}