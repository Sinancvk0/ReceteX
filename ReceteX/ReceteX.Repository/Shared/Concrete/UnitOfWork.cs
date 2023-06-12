using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Abstract;
using ReceteX.Repository.Concrete;
using ReceteX.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Shared.Concrete
{
	public class UnitOfWork : IUnitOfWork
	{
		public IRepository<AppUser> Users { get; private set; }

		public ICustomerRepository Customers { get; private set; }

		public IRepository<Description> Descriptions { get; private set; }

		public IRepository<DescriptionType> DescriptionTypes { get; private set; }

		public IRepository<Diagnosis> Diagnoses { get; private set; }

		public IRepository<Medicine> Medicines { get; private set; }

		public IRepository<MedicineUsagePeriod> MedicineUsagePeriods { get; private set; }

		public IRepository<MedicineUsageType> MedicineUsageTypes { get; private set; }

		public IRepository<Prescription> Prescriptions { get; private set; }

		public IRepository<PrescriptionsMedicine> PrescriptionMedicines { get; private set; }

		public IRepository<Status> Statuses { get; private set; }

		IRepository<PrescriptionsMedicine> IUnitOfWork.PrescriptionMedicines => throw new NotImplementedException();

		private readonly ApplicationDbContext _db;
		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Users = new Repository<AppUser>(db);
			Customers = new CustomerRepository(db);
			Descriptions = new Repository<Description>(db);
			DescriptionTypes = new Repository<DescriptionType>(db);
			Diagnoses = new Repository<Diagnosis>(db);
			Medicines = new Repository<Medicine>(db);
			MedicineUsagePeriods = new Repository<MedicineUsagePeriod>(db);
			MedicineUsageTypes = new Repository<MedicineUsageType>(db);
			Prescriptions = new Repository<Prescription>(db);
			PrescriptionMedicines = new Repository<PrescriptionsMedicine>(db);
			Statuses = new Repository<Status>(db);
		}


		public void Save()
		{
			_db.SaveChanges();
		}
	}
}