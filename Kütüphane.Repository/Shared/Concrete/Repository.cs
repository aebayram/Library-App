using Kutuphane.Data;
using Kutuphane.Models;
using Kutuphane.Repository.Shared.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Repository.Shared.Concrete
{
	public class Repository<T> : IRepository<T> where T : BaseModel
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			dbSet= db.Set<T>();
		}
		public void Add(T entity)
		{
			dbSet.Add(entity);

		}

		public void AddRange(IEnumerable<T> entities)
		{
			dbSet.AddRange(entities);
		}

		public virtual IQueryable<T> GetAll()
		{
			return dbSet.Where(x => x.isDeleted == false);
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
		{
			return GetAll().Where(filter);
		}

		public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
		{
			return GetAll().FirstOrDefault(filter);
		}

		public void Remove(T entity)
		{
			entity.isDeleted = true;
			entity.DateModified = DateTime.Now;
			dbSet.Update(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			foreach (T entity in entities)
			{
				entity.isDeleted = true;
				entity.DateModified = DateTime.Now;
			}
			dbSet.UpdateRange(entities);
		}

		public void Update(T entity)
		{
			entity.DateModified = DateTime.Now;
			dbSet.Update(entity);
		}

		public void UpdateRange(IEnumerable<T> entities)
		{
			foreach (T entity in entities)
			{
				entity.DateModified = DateTime.Now;
			}
			dbSet.UpdateRange(entities);
		}

	}
}