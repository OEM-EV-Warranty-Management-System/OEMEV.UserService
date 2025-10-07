using OEMEV.UserService.Application.Dtos;

namespace OEMEV.UserService.Application.Interfaces
{
	public interface IUserService
	{
		Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
		Task<Result<int>> AddUserAsync(UserDto userDto);
		Task<Result<List<UserDto>>> GetAllUsersAsync();
	}
}
