using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
	public class BaseModel
	{
		public Guid Id { get; set; } = new Guid();

		public string? Name { get; set; }

		public bool isDeleted { get; set; } = false;

		public DateTime DateCreated { get; set; }=DateTime.Now;


	}
}
