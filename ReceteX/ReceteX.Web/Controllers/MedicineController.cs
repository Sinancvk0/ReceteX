using Microsoft.AspNetCore.Mvc;
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
		public async Task< IActionResult> UpdateMedicineList()
		{
		 string content= await xmlRetriever.GetXmlContent("https://www.iby.com.tr/exe/ilaclar.xml");
			await ParseAndSaveFromXml(content);
			return RedirectToAction("Index");
		}
		

		public IActionResult Index()
		{
			return View();
		}
	}
}
