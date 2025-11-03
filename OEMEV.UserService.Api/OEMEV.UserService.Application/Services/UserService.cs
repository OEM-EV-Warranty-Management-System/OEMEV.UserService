using Microsoft.Extensions.Options;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Application.Mappers;
using OEMEV.UserService.Infrastructure.Base;
using OEMEV.UserService.Infrastructure.Interfaces;
using OEMEV.UserService.Infrastructure.Libraries;
using System.Security.Claims;
using System.Security.Cryptography;

namespace OEMEV.UserService.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repo;
		private readonly IEmailService _emailService;
		private readonly JWTSettings _jwtSettings;

		public UserService(IUserRepository repo, IOptions<JWTSettings> jwtSettings, IEmailService emailService)
		{
			_repo = repo;
			_jwtSettings = jwtSettings.Value
				?? throw new ArgumentNullException(nameof(jwtSettings), "JWT settings is not configured.");
			_emailService = emailService;
		}

		public async Task<Result<UserDto>> AddUserAsync(UserDto userDto)
		{
			try
			{
				var user = UserMappers.ToEntity(userDto);
				if (userDto.RoleId != 5)
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

		public async Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto)
		{
			try
			{
				var (accessTokenPrincipal, accessTokenError) = Authentication.ValidateToken(refreshTokenRequestDto.AccessToken, _jwtSettings, validateLifetime: false);
				if (accessTokenPrincipal == null)
				{
					return Result<LoginResponseDto>.Fail(accessTokenError ?? "Invalid access token.");
				}

				var (refreshTokenPrincipal, refreshTokenError) = Authentication.ValidateToken(refreshTokenRequestDto.RefreshToken, _jwtSettings, validateLifetime: true);
				if (refreshTokenPrincipal == null)
				{
					return Result<LoginResponseDto>.Fail(refreshTokenError ?? "Invalid or expired refresh token.");
				}

				var accessTokenType = accessTokenPrincipal.GetTokenType();
				var refreshTokenType = refreshTokenPrincipal.GetTokenType();
				if (accessTokenType != "access" || refreshTokenType != "refresh")
				{
					return Result<LoginResponseDto>.Fail("Invalid token types.");
				}

				var userIdFromAccessToken = accessTokenPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
				var userIdFromRefreshToken = refreshTokenPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userIdFromAccessToken) || userIdFromAccessToken != userIdFromRefreshToken)
				{
					return Result<LoginResponseDto>.Fail("Token mismatch.");
				}

				if (!Guid.TryParse(userIdFromAccessToken, out var userId))
				{
					return Result<LoginResponseDto>.Fail("Invalid user identifier in token.");
				}

				var (user, userError) = await _repo.GetByIdAsync(userId);
				if (user == null || userError != null)
				{
					return Result<LoginResponseDto>.Fail(userError ?? "User not found associated with this token.");
				}

				if (!user.IsActive)
				{
					return Result<LoginResponseDto>.Fail("User is inactive.");
				}

				if (user.RefreshToken != refreshTokenRequestDto.RefreshToken)
				{
					user.RefreshToken = null;
					await _repo.UpdateAsync(user);
					return Result<LoginResponseDto>.Fail("Refresh token has been revoked.");
				}

				var newAccessToken = Authentication.CreateAccessToken(user, _jwtSettings);
				if (newAccessToken.error != null)
				{
					return Result<LoginResponseDto>.Fail(newAccessToken.error);
				}

				var newRefreshToken = Authentication.CreateRefreshToken(user, _jwtSettings);
				if (newRefreshToken.error != null)
				{
					return Result<LoginResponseDto>.Fail(newRefreshToken.error);
				}

				user.RefreshToken = newRefreshToken.token!;
				var (updateResult, updateError) = await _repo.UpdateAsync(user);

				if (updateError != null || updateResult == null)
				{
					return Result<LoginResponseDto>.Fail(updateError ?? "Could not update refresh token.");
				}

				var response = new LoginResponseDto
				{
					AccessToken = newAccessToken.token!,
					RefreshToken = newRefreshToken.token!,
					Error = null
				};

				return Result<LoginResponseDto>.Ok(response);
			}
			catch (Exception ex)
			{
				return Result<LoginResponseDto>.Fail($"UserService.RefreshTokenAsync error: {ex.Message}");
			}
		}

		public async Task<Result<string>> ForgotPasswordAsync(string email)
		{
			try
			{
				var (user, error) = await _repo.GetByEmailAsync(email);
				if (user == null || error != null)
				{
					// Không tiết lộ thông tin người dùng tồn tại hay không
					return Result<string>.Ok("If an account with this email exists, an OTP has been sent.");
				}

				// Sinh OTP ngẫu nhiên 6 chữ số
				var random = new Random();
				var otp = random.Next(100000, 999999).ToString();

				// Lưu OTP và thời gian hết hạn
				user.PasswordResetToken = otp; // reuse field PasswordResetToken
				user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(10); // OTP hết hạn sau 10 phút

				var (updateResult, updateError) = await _repo.UpdateAsync(user);
				if (updateResult == null || updateError != null)
				{
					return Result<string>.Fail(updateError ?? "Failed to save OTP.");
				}

				// Tạo nội dung email
				var message = $@"
            <p>Dear {user.FullName},</p>
            <p>Your password reset OTP is:</p>
            <h2 style='color:#2d6cdf;font-size:24px;font-weight:bold;'>{otp}</h2>
            <p>This OTP will expire in 10 minutes.</p>
            <p>If you did not request a password reset, please ignore this email.</p>";

				await _emailService.SendEmailAsync(email, "Your Password Reset OTP", message);

				return Result<string>.Ok("If an account with this email exists, an OTP has been sent.");
			}
			catch (Exception)
			{
				return Result<string>.Fail("An error occurred while processing your request.");
			}
		}


		public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordDto)
		{
			try
			{
				var (user, error) = await _repo.GetByEmailAsync(resetPasswordDto.Email);
				if (user == null || error != null)
				{
					return Result<string>.Fail("Invalid OTP or email.");
				}

				// Kiểm tra OTP và hạn sử dụng
				if (user.PasswordResetToken != resetPasswordDto.Otp || user.ResetTokenExpires < DateTime.UtcNow)
				{
					return Result<string>.Fail("Invalid or expired OTP.");
				}

				// Cập nhật mật khẩu mới
				user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
				user.PasswordResetToken = null;
				user.ResetTokenExpires = null;

				var (updateResult, updateError) = await _repo.UpdateAsync(user);
				if (updateResult == null || updateError != null)
				{
					return Result<string>.Fail(updateError ?? "Failed to reset password.");
				}

				return Result<string>.Ok("Your password has been reset successfully.");
			}
			catch (Exception)
			{
				return Result<string>.Fail("An error occurred while resetting your password.");
			}
		}
	}
}