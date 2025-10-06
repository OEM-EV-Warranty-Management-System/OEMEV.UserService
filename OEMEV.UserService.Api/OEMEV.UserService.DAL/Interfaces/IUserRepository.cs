using OEMEV.UserService.Data.Models;

namespace OEMEV.UserService.DAL.Interfaces
{
	public interface IUserRepository
	{
		Task<(IEnumerable<User> Users, string? Error)> GetAllAsync();
		Task<(User? User, string? Error)> GetByUserNameAsync(string userName);
		Task<(int Result, string? Error)> AddAsync(User user);
		Task<(int Result, string? Error)> UpdateAsync(User user);
	}
}
