using Microsoft.EntityFrameworkCore;
using OEMEV.UserService.DAL.Interfaces;
using OEMEV.UserService.Data.DBContext;
using System.Linq.Expressions;

namespace OEMEV.UserService.DAL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly AppDbContext _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task UpdateAsync(T entity)
		{
			await Task.Run(() => _dbSet.Update(entity));
		}

		public async Task DeleteAsyncById(object id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
			}
		}
		public async Task DeleteAsync(params object[] keyValues)
		{
			var entity = await _dbSet.FindAsync(keyValues);
			if (entity != null)
			{
				_dbSet.Remove(entity);
			}
		}
		public async Task<List<T>> GetAllByPropertyAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp.Trim());
				}
			}

			return await query.ToListAsync();
		}

		public async Task<T?> GetByPropertyAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;
			if (!tracked)
			{
				query = query.AsNoTracking();
			}
			if (includeProperties != null)
			{
				foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp.Trim());
				}
			}

			if (filter != null)
			{
				query = query.Where(filter);
			}
			return await query.FirstOrDefaultAsync();
		}
	}
}
