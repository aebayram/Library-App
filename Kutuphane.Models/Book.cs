using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Models
{
	public class Book : BaseModel
	{
		public float Price { get; set; }
		public int TotalPages { get; set; }
		public string Isbn { get; set; }
		public DateTime PublishDate { get; set; }
		public virtual ICollection<Author> Authors { get; set; }
		public Guid CategoryId { get; set; }
		public virtual Category Category { get; set; }
		public virtual ICollection<Publisher> Publishers { get; set; }

	}
}