using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
	public class PrescriptionsMedicine : BaseModel
	{
		public Guid PrescriptionId { get; set; }
		public virtual Prescription? Prescription { get; set; }
		public Guid MedicineId { get; set; }
		public virtual Medicine? Medicine { get; set; }

		public int Quantity { get; set; }

		public int Dose1 { get; set; }
		public int Dose2 { get; set; }

		public int MedicineUsagePeriodId { get; set; }

		public virtual MedicineUsagePeriod? MedicineUsagePeriod { get; set; }
		public int MedicineUsageTypeId { get; set; }

		public virtual MedicineUsageType? MedicineUsageType { get; set; }
	}
}
