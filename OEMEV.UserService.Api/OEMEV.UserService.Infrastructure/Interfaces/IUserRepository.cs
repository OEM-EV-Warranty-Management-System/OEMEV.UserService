using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Infrastructure.Interfaces
{
	public interface IUserRepository
	{
		Task<(IEnumerable<User> Users, string? Error)> GetAllAsync();
		Task<(User? User, string? Error)> GetByUserNameAsync(string userName);
		Task<(User? User, string? Error)> GetByIdAsync(Guid id);
		Task<(User? User, string? Error)> AddAsync(User user);
		Task<(User? User, string? Error)> UpdateAsync(User user);
	}
}