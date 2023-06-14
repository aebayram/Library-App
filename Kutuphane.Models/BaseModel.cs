using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Models
{
	public class BaseModel
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required(ErrorMessage = "İsim girmek zorunludur")]
		public string Name { get; set; }
		public string? Description { get; set; }
		public bool isDeleted { get; set; } = false;
		public bool isActive { get; set; } = true;
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime? DateModified { get; set; }


	}
}