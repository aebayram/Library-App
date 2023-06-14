using Kutuphane.Models;
using Kutuphane.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Repository.Shared.Abstract
{
	public interface IUnitOfWork
	{
		IRepository<Author> Authors { get; }
		IRepository<Publisher> Publishers { get; } 
		// Ya da şu şekilde yazılablir IPublisherRepository Publishers { get; }
		IRepository<Category> Categories { get; }
		IRepository<AppUser> Users { get; }
		IBookRepository Books { get; }

		void Save();
	}
}