﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
	public class AppUser : BaseModel
	{
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? Gsm { get; set; }
		public string? TCKN { get; set; }
		public string? PinCOde { get; set; }
		public int? CityCode { get; set; }
		public string MedulaPassword { get; set; }


	}
}
