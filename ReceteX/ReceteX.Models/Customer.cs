using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
	public class Customer:BaseModel
	{
		public ICollection <AppUser>? AppUsers { get; set; }

	}
}
