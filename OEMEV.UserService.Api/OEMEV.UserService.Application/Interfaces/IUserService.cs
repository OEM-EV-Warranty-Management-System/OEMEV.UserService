﻿using OEMEV.UserService.Application.Dtos;

namespace OEMEV.UserService.Application.Interfaces
{
	public interface IUserService
	{
		Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
		Task<Result<UserDto>> AddUserAsync(UserDto userDto);
		Task<Result<List<UserDto>>> GetAllUsersAsync();
		Task<Result<UserDto>> GetUserByIdAsync(Guid id);
		Task<Result<UserDto>> UpdateUserAsync(UserDto userDto);
		Task<Result<int>> DeleteUserAsync(Guid id);
	}
}