using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Infrastructure.Interfaces
{
	public interface IRoleRepository
	{
		Task<(IEnumerable<Role> Role, string? Error)> GetAllAsync();
		Task<(Role? Role, string? Error)> GetByIdAsync(long id);
		Task<(Role? Role, string? Error)> CreateAsync(Role role);
		Task<(Role? Role, string? Error)> UpdateAsync(Role role);
	}
}
