using Kutuphane.Models;
using Kutuphane.Repository.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Repository.Abstract
{
	public interface IBookRepository : IRepository<Book>
	{
		void RemoveAuthorFromBook(Guid bookId, Guid authorId);
		void removePublisherFromBook(Guid bookId, Guid publisherId);
		void AddAuthorToBook(Guid bookId, List<Guid> authors);
		void AddPublisherToBook(Guid bookId, List<Publisher> publishers);
	}
}