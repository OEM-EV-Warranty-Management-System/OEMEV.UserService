using OEMEV.UserService.BLL.Dtos;

namespace OEMEV.UserService.BLL.Interfaces
{
	public interface IUserService
	{
		Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
		Task<Result<int>> AddUserAsync(UserDto userDto);
		Task<Result<List<UserDto>>> GetAllUsersAsync();
	}
}
