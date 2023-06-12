using ReceteX.Models;
using ReceteX.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Shared.Abstract
{
	public interface IUnitOfWork
	{
		IRepository<AppUser> Users { get; }
		ICustomerRepository Customers { get; }
		IRepository<Description> Descriptions { get; }
		IRepository<DescriptionType> DescriptionTypes { get; }
		IRepository<Diagnosis> Diagnoses { get; }
		IRepository<Medicine> Medicines { get; }
		IRepository<MedicineUsagePeriod> MedicineUsagePeriods { get; }
		IRepository<MedicineUsageType> MedicineUsageTypes { get; }
		IRepository<Prescription> Prescriptions { get; }
		IRepository<PrescriptionsMedicine> PrescriptionMedicines { get; }
		IRepository<Status> Statuses { get; }
		void Save();
	}
}