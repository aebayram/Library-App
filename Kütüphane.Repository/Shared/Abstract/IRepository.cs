using Kutuphane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Repository.Shared.Abstract
{
	public interface IRepository<T> where T : BaseModel
	{
		IQueryable<T> GetAll();
		IQueryable<T> GetAll(Expression<Func<T, bool>> filter);
		void Add(T entity);
		T GetFirstOrDefault(Expression<Func<T, bool>> filter);
		void Update(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
		void AddRange(IEnumerable<T> entities);
		void UpdateRange(IEnumerable<T> entities);

		
	}
}