using Microsoft.AspNetCore.Mvc;
using OEMEV.UserService.BLL.Dtos;
using OEMEV.UserService.BLL.Interfaces;

namespace OEMEV.UserService.Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IServiceProviders _serviceProvides;
		public UserController(IServiceProviders serviceProviders)
		{
			_serviceProvides = serviceProviders;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var result = await _serviceProvides.UserService.GetAllUsersAsync();
			if (!result.Success) return BadRequest(new { message = result.Error });

			return Ok(result.Data);
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _serviceProvides.UserService.LoginAsync(loginDto);
			if (!result.Success) return Unauthorized(new { message = result.Error });

			return Ok(result.Data);
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _serviceProvides.UserService.AddUserAsync(userDto);
			if (!result.Success) return BadRequest(new { message = result.Error });
			return CreatedAtAction(nameof(GetAllUsers), new { id = result.Data }, new { result = "User created successfully" });
		}
	}
}
