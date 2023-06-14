using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
	public class Diagnosis:BaseModel
	{
		public string? Code { get; set; }	
		public ICollection<Prescription> Prescription { get; set; } 

	}
}
