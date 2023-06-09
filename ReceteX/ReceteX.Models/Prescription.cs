using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
	public class Prescription:BaseModel
	{
		public Guid AppUserId { get; set; }
		public virtual AppUser? AppUser { get; set; }
		public Guid StatusId { get; set; }
		public virtual Status? Status { get; set; }



		public ICollection<PrescriptionsMedicine>? PrescriptionsMedicines { get; set; }

		public ICollection<Description>? Descriptions { get; set; }

		public ICollection<Diagnosis>? Diagnoses { get; set; }

		public string? PatientFirsName { get; set; }
		public string? PatientLastName { get; set; }
		public string? PatientGsm { get; set; }
		public string? TCKN { get; set; }
		public string?Gender { get; set; } = "E";
		public string? BirthDate { get; set; } = "01.01.1985";
		public string? PrescriptionNo { get; set; }

	




	}
}
