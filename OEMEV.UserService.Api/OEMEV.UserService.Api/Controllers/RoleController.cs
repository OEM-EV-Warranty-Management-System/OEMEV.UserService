using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Api.Controllers
{
	[Authorize (Roles = "1")]
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IServiceProviders _serviceProviders;

		public RoleController(IServiceProviders serviceProviders)
		{
			_serviceProviders = serviceProviders;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _serviceProviders.RoleService.GetAllAsync();
			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}
			return Ok(result.Data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(long id)
		{
			var result = await _serviceProviders.RoleService.GetByIdAsync(id);
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

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] RoleDto roleDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userName = this.User.Identity?.Name;
			if (string.IsNullOrEmpty(userName))
			{
				return Unauthorized(new { message = "User is not authenticated." });
			}

			roleDto.CreatedBy = userName;

			var result = await _serviceProviders.RoleService.CreateAsync(roleDto);

			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}

			return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] long id, [FromBody] RoleDto roleDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userName = this.User?.Identity?.Name;
			if (string.IsNullOrEmpty(userName))
			{
				return Unauthorized(new { message = "User is not authenticated." });
			}

			roleDto.Id = id;
			roleDto.UpdatedBy = userName;

			var result = await _serviceProviders.RoleService.UpdateAsync(roleDto);
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

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] long id)
		{
			var userName = this.User?.Identity?.Name;
			if (string.IsNullOrEmpty(userName))
			{
				return Unauthorized(new { message = "User is not authenticated." });
			}

			var result = await _serviceProviders.RoleService.DeleteAsync(id, userName);
			if (!result.Success)
			{
				if (result.Error != null && result.Error.Contains("not found"))
				{
					return NotFound(new { message = result.Error });
				}
				return BadRequest(new { message = result.Error });
			}
			return Ok(new { message = "Role deleted successfully." });
		}
	}
}