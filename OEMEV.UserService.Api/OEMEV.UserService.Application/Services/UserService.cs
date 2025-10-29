using Microsoft.Extensions.Options;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Application.Mappers;
using OEMEV.UserService.Infrastructure.Base;
using OEMEV.UserService.Infrastructure.Interfaces;
using OEMEV.UserService.Infrastructure.Libraries;

namespace OEMEV.UserService.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repo;
		private readonly JWTSettings _jwtSettings;

		public UserService(IUserRepository repo, IOptions<JWTSettings> jwtSettings)
		{
			_repo = repo;
			_jwtSettings = jwtSettings.Value
				?? throw new ArgumentNullException(nameof(jwtSettings), "JWT settings is not configured.");
		}

		public async Task<Result<UserDto>> AddUserAsync(UserDto userDto)
		{
			try
			{
				var user = UserMappers.ToEntity(userDto);
				if(userDto.RoleId != 5)
				{
					user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("@1");
				}

				var (result, error) = await _repo.AddAsync(user);

				if (error != null || result == null)
					return Result<UserDto>.Fail(error ?? "Can not save this user!");

				return Result<UserDto>.Ok(UserMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<UserDto>.Fail($"UserService.AddUserAsync error: {ex.Message}");
			}
		}

		public async Task<Result<int>> DeleteUserAsync(Guid id)
		{
			try
			{
				var (user, error) = await _repo.GetByIdAsync(id);
				if (error != null) return Result<int>.Fail(error);
				if (user == null) return Result<int>.Fail("User not found.");

				user.IsActive = false;
				var (updatedUser, updateError) = await _repo.UpdateAsync(user);

				if (updateError != null) return Result<int>.Fail(updateError);

				return Result<int>.Ok(1);
			}
			catch (Exception ex)
			{
				return Result<int>.Fail($"UserService.DeleteUserAsync error: {ex.Message}");
			}
		}

		public async Task<Result<List<UserDto>>> GetAllUsersAsync()
		{
			try
			{
				var (users, error) = await _repo.GetAllAsync();
				if (error != null)
					return Result<List<UserDto>>.Fail(error);

				var userDtos = users.Select(UserMappers.ToDto).ToList();
				return Result<List<UserDto>>.Ok(userDtos);
			}
			catch (Exception ex)
			{
				return Result<List<UserDto>>.Fail($"UserService.GetAllUsersAsync error: {ex.Message}");
			}
		}

		public async Task<Result<UserDto>> GetUserByIdAsync(Guid id)
		{
			try
			{
				var (user, error) = await _repo.GetByIdAsync(id);
				if (error != null) return Result<UserDto>.Fail(error);
				if (user == null) return Result<UserDto>.Fail("User not found.");

				return Result<UserDto>.Ok(UserMappers.ToDto(user));
			}
			catch (Exception ex)
			{
				return Result<UserDto>.Fail($"UserService.GetUserByIdAsync error: {ex.Message}");
			}
		}

		public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
		{
			try
			{
				var (user, error) = await _repo.GetByUserNameAsync(loginRequestDto.UserName);
				if (error != null)
					return Result<LoginResponseDto>.Fail(error);

				if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash))
					return Result<LoginResponseDto>.Fail("Incorrect username or password!");

				var accessToken = Authentication.CreateAccessToken(user, _jwtSettings);
				if (accessToken.error != null)
					return Result<LoginResponseDto>.Fail(accessToken.error);

				var refreshToken = Authentication.CreateRefreshToken(user, _jwtSettings);
				if (refreshToken.error != null)
					return Result<LoginResponseDto>.Fail(refreshToken.error);

				user.RefreshToken = refreshToken.token!;
				var (updateResult, updateError) = await _repo.UpdateAsync(user);

				if (updateError != null || updateResult == null)
					return Result<LoginResponseDto>.Fail(updateError ?? "Could not update refresh token!");

				var response = new LoginResponseDto
				{
					AccessToken = accessToken.token!,
					RefreshToken = refreshToken.token!,
					Error = null
				};

				return Result<LoginResponseDto>.Ok(response);
			}
			catch (Exception ex)
			{
				return Result<LoginResponseDto>.Fail($"UserService.LoginAsync error: {ex.Message}");
			}
		}

		public async Task<Result<UserDto>> UpdateUserAsync(UserDto userDto)
		{
			try
			{
				var (userToUpdate, error) = await _repo.GetByIdAsync(userDto.Id.Value);
				if (error != null) return Result<UserDto>.Fail(error);
				if (userToUpdate == null) return Result<UserDto>.Fail("User not found.");

				userToUpdate.FullName = userDto.FullName;
				userToUpdate.PhoneNumber = userDto.PhoneNumber;
				userToUpdate.Email = userDto.Email;
				userToUpdate.RoleId = userDto.RoleId;
				userToUpdate.ServiceCenterId = userDto.ServiceCenterId;


				var (updatedUser, updateError) = await _repo.UpdateAsync(userToUpdate);
				if (updateError != null || updatedUser == null)
				{
					return Result<UserDto>.Fail(updateError ?? "Failed to update user.");
				}

				return Result<UserDto>.Ok(UserMappers.ToDto(updatedUser));
			}
			catch (Exception ex)
			{
				return Result<UserDto>.Fail($"UserService.UpdateUserAsync error: {ex.Message}");
			}
		}
	}
}