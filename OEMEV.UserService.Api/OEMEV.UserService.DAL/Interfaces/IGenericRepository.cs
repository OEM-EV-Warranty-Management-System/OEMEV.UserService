﻿using System.Linq.Expressions;

namespace OEMEV.UserService.DAL.Interfaces
{
	public interface IGenericRepository <T> where T : class
	{
		Task AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsyncById(object id);
		Task DeleteAsync(params object[] keyValues);
		Task<List<T>> GetAllByPropertyAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		Task<T?> GetByPropertyAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
	}
}
