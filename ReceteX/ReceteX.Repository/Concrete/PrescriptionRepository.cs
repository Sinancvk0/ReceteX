using Microsoft.EntityFrameworkCore;
using ReceteX.Data;
using ReceteX.Models;
using ReceteX.Repository.Abstract;
using ReceteX.Repository.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Repository.Concrete
{
	public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
	{
		private readonly ApplicationDbContext _db;

		public PrescriptionRepository(ApplicationDbContext db):base(db)
		{
			_db = db;
		}

		public override Prescription GetFirstOrDefault(Expression<Func<Prescription, bool>> filter)
		{
			return _db.Prescriptions.Include(p => p.Diagnoses).Include(p => p.PrescriptionMedicines).Include(p => p.Descriptions).FirstOrDefault(filter);

		}
	}
}
