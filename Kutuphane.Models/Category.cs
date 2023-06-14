
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Models
{
	public class Category : BaseModel
	{
		public virtual ICollection<Book> Books { get; set; }
		//ds
	}
}