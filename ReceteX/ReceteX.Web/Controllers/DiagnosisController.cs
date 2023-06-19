using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            XmlNodeList diagnosesFromXml = xmlDoc.SelectNodes("/tanilar/tani");
            //datatabse'deki tüm aktif nesneler
            IQueryable<Diagnosis> diagnosesFromDb = unitOfWork.Diagnoses.GetAll().AsNoTracking().OrderBy(m => m.Code).ToList().AsQueryable<Diagnosis>();

            //database'deki tüm silinmiş nesneler
            IQueryable<Diagnosis> deletedDiagnosesFromDb = unitOfWork.Diagnoses.GetAllDeleted().OrderBy(m => m.Code).ToList().AsQueryable<Diagnosis>();

            //Yeni kayıtları aktaran döngümüz.
            foreach (XmlNode diagnosis in diagnosesFromXml)
            {
                string codeFromXml = diagnosis.SelectSingleNode("kod").InnerText;

                if (!diagnosesFromDb.Any(m => m.Code == codeFromXml))
                {
                    Diagnosis diag = new Diagnosis();
                    diag.Name = diagnosis.SelectSingleNode("ad").InnerText;
                    diag.Code = codeFromXml;
                    unitOfWork.Diagnoses.Add(diag);
                }
                else
                {
                    Diagnosis medSilinmis = deletedDiagnosesFromDb.FirstOrDefault(m => m.Code == codeFromXml);
                    if (medSilinmis != null)
                    {
                        medSilinmis.isDeleted = false;
                        unitOfWork.Diagnoses.Update(medSilinmis);
                    }
                }
            }

            unitOfWork.Save();

            //Kaynaktan silinmiş olan ilaçları databasede isdelete=true yapan döngümüz
            IEnumerable<XmlNode> diagnosesFromXmlEnumarable = xmlDoc.SelectNodes("/tanilar/tani").Cast<XmlNode>();

            foreach (Diagnosis tani in diagnosesFromDb)
            {
                if (!diagnosesFromXmlEnumarable.Any(x => x.SelectSingleNode("kod").InnerText == tani.Code))
                {
                    tani.isDeleted = true;
                    unitOfWork.Diagnoses.Update(tani);
                }

            }


            unitOfWork.Save();

        }

        public async Task<IActionResult> UpdateDiagnosesList()
        {
            string content = await xmlRetriever.GetXmlContent("https://www.ibys.com.tr/exe/tanilar.xml");
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
            return Json(new { data = unitOfWork.Diagnoses.GetAll() });
        }

        [HttpGet]
        public JsonResult SearchDiagnosis(string searchTerm)
        {
            var diagnoses = unitOfWork.Diagnoses.GetAll(d => d.Name.ToLower().Contains(searchTerm.ToLower()) || d.Code.ToLower().Contains(searchTerm.ToLower())).Select(d => new { d.Id, Name = d.Code + " - " + d.Name }).ToList();

            return Json(diagnoses);
        }


    }
}