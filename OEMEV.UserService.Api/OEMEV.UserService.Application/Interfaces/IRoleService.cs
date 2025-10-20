using OEMEV.UserService.Application.Dtos;

namespace OEMEV.UserService.Application.Interfaces
{
	public interface IRoleService
	{
		Task<Result<List<RoleDto>>> GetAllAsync();
		Task<Result<RoleDto>> GetByIdAsync(long id);
		Task<Result<RoleDto>> CreateAsync(RoleDto roleDto);
		Task<Result<RoleDto>> UpdateAsync(RoleDto roleDto);
		Task<Result<int>> DeleteAsync(long id, string updatedBy);
	}
}