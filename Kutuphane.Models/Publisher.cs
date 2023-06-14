using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Models
{
	public class Publisher : BaseModel
	{
		public virtual ICollection<Book> Books { get; set; }

	}
}