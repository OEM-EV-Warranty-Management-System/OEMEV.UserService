using OEMEV.UserService.Infrastructure.DBContext;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		public UnitOfWork(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IGenericRepository<T> GetRepository<T>() where T : class
		{
			return new GenericRepository<T>(_dbContext);
		}
		public async Task<int> SaveAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}
