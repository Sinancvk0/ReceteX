using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			IQueryable<Medicine> medicinesFromDb = unitOfWork.Medicines.GetAll().AsNoTracking().OrderBy(m => m.Name).ToList().AsQueryable<Medicine>(); //databasedeki erşeyi bi kere çağırdık.Önce ToList le liste yaptık tekrar queryable yaptık

			//database'deki tüm silinmiş nesneler
			IQueryable<Medicine> deletedMedicinesFromDb = unitOfWork.Medicines.GetAllDeleted().AsNoTracking().OrderBy(m => m.Name).ToList().AsQueryable<Medicine>();


			//yeni kayıtları aktaran döngümüz
			foreach (XmlNode medicine in medicinesFromXml)
			{
				string barcodeFromXml = medicine.SelectSingleNode("barkod").InnerText;

				if (!medicinesFromDb.Any(m => m.Barcode == barcodeFromXml))  //bu şekilde yaparak güncelle dedik mi değişiklik yoksa databaseye veri ekleme çıkarma olmayacak. Any bu barcode sahip kayıp var mı diye sorma amacıyla kullandık.yani kayıt yoksa ekle diyoruz.
				{
					Medicine med = new Medicine();
					med.Name = medicine.SelectSingleNode("ad").InnerText;  //ad ve barkod ismi var ilaçlarda burda adı çekiyoruz
					med.Barcode = barcodeFromXml;
					unitOfWork.Medicines.Add(med); //oluşturduğumuz Medicines nesnesini ekliyoruz
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



			//kaynaktan silinmiş olan ilaçları databasede isdelete=true yapan dögümüz
			IEnumerable<XmlNode> medicinesFromXmlEnumarable = xmlDoc.SelectNodes("/ilaclar/ilac").Cast<XmlNode>();
			foreach (Medicine ilac in medicinesFromDb)
			{
				if (!medicinesFromXmlEnumarable.Any(x => x.SelectSingleNode("barkod").InnerText == ilac.Barcode))
				{
					ilac.isDeleted = true;
					unitOfWork.Medicines.Update(ilac); //yukardakini yazmadık çünkü zaten remove methodumuz onu yapıyor
				}
			}


			unitOfWork.Save();  // bu komut foreach in içinde olsaydı ilaçlar databaseye 3 dakikada gelecekti.buraya yazdık ve saniyeler içinde geldi
		}

		public async Task<IActionResult> UpdateMedicinesList()
		{
			string content = await xmlRetriever.GetXmlContent("https://www.ibys.com.tr/exe/ilaclar.xml");
			await ParseAndSaveFromXml(content);

			return RedirectToAction("index");
		}



		public IActionResult Index()
		{
			return View();
		}
	}
}