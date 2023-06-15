using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Utility;
using System.Xml;

namespace ReceteX.Web.Controllers
{
	public class DiagnosisController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly XmlRetriever xmlRetriever;

		public DiagnosisController(IUnitOfWork unitOfWork, XmlRetriever xmlRetriever)
		{
			this.unitOfWork = unitOfWork;
			this.xmlRetriever = xmlRetriever;
		}

		public async Task ParseAndSaveFromXml(string xmlContent)
		{	
			XmlDocument xmlDoc =new XmlDocument();
			xmlDoc.LoadXml(xmlContent);
			XmlNodeList diagnoses = xmlDoc.SelectNodes("tanilar/tani");

			foreach (XmlNode diagnosis  in diagnoses)
			{

				Diagnosis dia = new Diagnosis();

				dia.Code = diagnosis.SelectSingleNode("kod").InnerText;

				dia.Name = diagnosis.SelectSingleNode("ad").InnerText;

				unitOfWork.Diagnoses.Add(dia);
			}

			unitOfWork.Save();


		}

		public async Task<IActionResult> UpdateDiagnosisList()
		{
			string content = await xmlRetriever.GetXmlContent("https://www.ibys.com.tr/exe/tanilar.xml");
			await ParseAndSaveFromXml(content);
			return RedirectToAction("Index");
		}



		public IActionResult Index()
		{
			return View();
		}
	}
}
