using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
		{


		}
		public virtual DbSet<AppUser>

	}
}
