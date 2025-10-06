using OEMEV.UserService.DAL.Interfaces;
using OEMEV.UserService.Data.DBContext;

namespace OEMEV.UserService.DAL.Repositories
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
