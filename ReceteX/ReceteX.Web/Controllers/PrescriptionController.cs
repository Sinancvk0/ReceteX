using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;

namespace ReceteX.Web.Controllers
{
	[Authorize]
	public class PrescriptionController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		public PrescriptionController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		[HttpGet]
		[Route("Receteyaz/{id?}")]
		public IActionResult Write(string id)
		{
			Prescription prescription = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == Guid.Parse(id));
			return View(prescription);
		}

		[HttpPost]
		public IActionResult Create(Prescription prescription)
		{
			unitOfWork.Prescriptions.Add(prescription);
			unitOfWork.Save();
			return Json(prescription);
		}

		[HttpPost]
		public IActionResult AddDiagnosis(Guid prescriptionId, Guid diagnosisId)
		{
			Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
			Diagnosis asilDiagnosis = unitOfWork.Diagnoses.GetFirstOrDefault(d => d.Id == diagnosisId);
			asil.Diagnoses.Add(asilDiagnosis);
			unitOfWork.Prescriptions.Update(asil);
			unitOfWork.Save();
			return Ok();

		}

		[HttpPost]
		public IActionResult AddDescription(Guid prescriptionId, Description description)
		{
			Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
			asil.Descriptions.Add(description);
			unitOfWork.Prescriptions.Update(asil);

			unitOfWork.Save();
			return Ok();
		}
		[HttpPost]
		public IActionResult AddMedicine(Guid prescriptionId, PrescriptionsMedicine prescriptionMedicine)
		{
			Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
			asil.PrescriptionMedicines.Add(prescriptionMedicine);
			unitOfWork.Prescriptions.Update(asil);
			unitOfWork.Save();
			return Ok();

		}

		[HttpPost]
		public IActionResult RemoveMedicine(Guid prescriptionId, Guid prescriptionMedicineId)
		{
			Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);

			asil.PrescriptionMedicines.Remove(asil.PrescriptionMedicines.FirstOrDefault(pm => pm.Id == prescriptionMedicineId));
			unitOfWork.Prescriptions.Update(asil);
			unitOfWork.Save();
			return Ok();

		}

		[HttpPost]
		public IActionResult RemoveDescription(Guid prescriptionId, Guid descriptionId)
		{
			Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
			asil.Descriptions.Remove(asil.Descriptions.FirstOrDefault(d => d.Id == descriptionId));
			unitOfWork.Prescriptions.Update(asil);
			unitOfWork.Save();
			return Ok();
		}
		[HttpPost]
		public IActionResult RemoveDiagnosis(Guid prescriptionId, Guid diagnosisId)
		{
			Prescription asil = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
			asil.Diagnoses.Remove(asil.Diagnoses.FirstOrDefault(d => d.Id == diagnosisId));
			unitOfWork.Prescriptions.Update(asil);
			unitOfWork.Save();
			return Ok();

		}

		
	}
}