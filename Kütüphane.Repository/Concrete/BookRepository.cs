using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Abstract;
using Kutuphane.Repository.Shared.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Repository.Concrete
{
	public class BookRepository : Repository<Book>, IBookRepository
	{
		private readonly ApplicationDbContext _db;

		public BookRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public override IQueryable<Book> GetAll()
		{
			return _db.Books.Include(b => b.Authors).Include(b => b.Publishers).Include(b => b.Category).Where(b => b.isDeleted == false);
		}

		public void AddAuthorToBook(Guid bookId, List<Guid> authors)
		{
			throw new NotImplementedException();
		}

		public void AddPublisherToBook(Guid bookId, List<Publisher> publishers)
		{
			throw new NotImplementedException();
		}

		public void RemoveAuthorFromBook(Guid bookId, Guid authorId)
		{
			throw new NotImplementedException();
		}

		public void removePublisherFromBook(Guid bookId, Guid publisherId)
		{
			throw new NotImplementedException();
		}
	}
}