using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Api.Controllers
{
	[Authorize(Roles = "1")]
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceCenterController : ControllerBase
	{
		private readonly IServiceProviders _serviceProviders;
		public ServiceCenterController(IServiceProviders serviceProviders)
		{
			_serviceProviders = serviceProviders;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _serviceProviders.ServiceCenterService.GetAllAsync();
			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}
			return Ok(result.Data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(long id)
		{
			var result = await _serviceProviders.ServiceCenterService.GetByIdAsync(id);
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
		public async Task<IActionResult> Create([FromBody] ServiceCenterDto serviceCenterDto)
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

			serviceCenterDto.CreatedBy = userName;

			var result = await _serviceProviders.ServiceCenterService.CreateAsync(serviceCenterDto);

			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}

			return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] long id, [FromBody] ServiceCenterDto serviceCenterDto)
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

			serviceCenterDto.Id = id;
			serviceCenterDto.UpdatedBy = userName;

			var result = await _serviceProviders.ServiceCenterService.UpdateAsync(serviceCenterDto);
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

			var result = await _serviceProviders.ServiceCenterService.DeleteAsync(id, userName);
			if (!result.Success)
			{
				if (result.Error != null && result.Error.Contains("not found"))
				{
					return NotFound(new { message = result.Error });
				}
				return BadRequest(new { message = result.Error });
			}
			return Ok(new { message = "Service center deleted successfully." });
		}
	}
}