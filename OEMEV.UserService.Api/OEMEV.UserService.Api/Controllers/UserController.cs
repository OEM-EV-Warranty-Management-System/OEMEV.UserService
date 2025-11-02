using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IServiceProviders _serviceProviders;
		public UserController(IServiceProviders serviceProviders)
		{
			_serviceProviders = serviceProviders;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _serviceProviders.UserService.LoginAsync(loginDto);
			if (!result.Success) return Unauthorized(new { message = result.Error });

			return Ok(result.Data);
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _serviceProviders.UserService.RefreshTokenAsync(refreshTokenDto);
			if (!result.Success)
			{
				return Unauthorized(new { message = result.Error });
			}

			return Ok(result.Data);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var result = await _serviceProviders.UserService.GetAllUsersAsync();
			if (!result.Success) return BadRequest(new { message = result.Error });

			return Ok(result.Data);
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(Guid id)
		{
			var result = await _serviceProviders.UserService.GetUserByIdAsync(id);
			if (!result.Success)
			{
				if (result.Error != null && result.Error.Contains("not found"))
				{
					return NotFound(new { message = result.Error });
				}
				return BadRequest(new { message = result.Error });
			}
			return Ok(result.Data);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (userDto.RoleId != 5 && String.IsNullOrEmpty(userDto.UserName))
				return BadRequest($"User with role {userDto.RoleId} need user name.");

			if ((userDto.RoleId == 3 || userDto.RoleId == 4) && userDto.ServiceCenterId == null)
				return BadRequest("Users with role 3 or 4 must have a ServiceCenterId.");

			if (userDto.RoleId == 2 && userDto.ManufacturerId == null)
				return BadRequest("Users with role 2 must have a ManufacturerId.");

			var result = await _serviceProviders.UserService.AddUserAsync(userDto);
			if (!result.Success) return BadRequest(new { message = result.Error });

			return CreatedAtAction(nameof(GetUserById), new { id = result.Data!.Id }, result.Data);
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			userDto.Id = id;
			var result = await _serviceProviders.UserService.UpdateUserAsync(userDto);
			if (!result.Success)
			{
				if (result.Error != null && result.Error.Contains("not found"))
				{
					return NotFound(new { message = result.Error });
				}
				return BadRequest(new { message = result.Error });
			}
			return Ok(result.Data);
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(Guid id)
		{
			var result = await _serviceProviders.UserService.DeleteUserAsync(id);
			if (!result.Success)
			{
				if (result.Error != null && result.Error.Contains("not found"))
				{
					return NotFound(new { message = result.Error });
				}
				return BadRequest(new { message = result.Error });
			}
			return Ok(new { message = "User deleted successfully." });
		}

		[Authorize]
		[HttpPatch("{id}/deactivate")]
		public async Task<IActionResult> Deactivate([FromRoute] Guid id, bool status)
		{
			var userDto = await _serviceProviders.UserService.GetUserByIdAsync(id);
			if (!userDto.Success)
			{
				if (userDto.Error != null && userDto.Error.Contains("not found"))
				{
					return NotFound(new { message = userDto.Error });
				}
				return BadRequest(new { message = userDto.Error });
			}

			var result = await _serviceProviders.UserService.UpdateUserAsync(userDto.Data);
			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}

			return Ok(result.Data);
		}
	}
}