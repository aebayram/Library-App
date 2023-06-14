using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Models
{
	[Table("Users")]
	public class AppUser : BaseModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool isAdmin { get; set; }

	}

}