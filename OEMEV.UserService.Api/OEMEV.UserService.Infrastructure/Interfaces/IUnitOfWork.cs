namespace OEMEV.UserService.Infrastructure.Interfaces
{
	public interface IUnitOfWork
	{
		IGenericRepository<T> GetRepository<T>() where T : class;
		Task<int> SaveAsync();
	}
}
