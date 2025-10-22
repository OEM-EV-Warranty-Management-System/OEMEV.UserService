using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ManufactureController : ControllerBase
	{
		private readonly IServiceProviders _serviceProviders;

		public ManufactureController(IServiceProviders serviceProviders)
		{
			_serviceProviders = serviceProviders;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _serviceProviders.ManufactureService.GetAllAsync();
			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}
			return Ok(result.Data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(long id)
		{
			var result = await _serviceProviders.ManufactureService.GetByIdAsync(id);
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
		public async Task<IActionResult> Create([FromBody] ManufactureDto manufactureDto)
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

			manufactureDto.CreatedBy = userName;

			var result = await _serviceProviders.ManufactureService.CreateAsync(manufactureDto);

			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}

			return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] long id, [FromBody] ManufactureDto manufactureDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			manufactureDto.Id = id;

			var result = await _serviceProviders.ManufactureService.UpdateAsync(manufactureDto);
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
			var result = await _serviceProviders.ManufactureService.DeleteAsync(id);
			if (!result.Success)
			{
				if (result.Error != null && result.Error.Contains("not found"))
				{
					return NotFound(new { message = result.Error });
				}
				return BadRequest(new { message = result.Error });
			}
			return Ok(new { message = "Manufacture deleted successfully." });
		}

		[HttpPatch("{id}/deactivate")]
		public async Task<IActionResult> Deactivate([FromRoute] long id, bool status)
		{
			var manufactureDto = await _serviceProviders.ManufactureService.GetByIdAsync(id);
			if (!manufactureDto.Success)
			{
				if (manufactureDto.Error != null && manufactureDto.Error.Contains("not found"))
				{
					return NotFound(new { message = manufactureDto.Error });
				}
				return BadRequest(new { message = manufactureDto.Error });
			}

			var userName = this.User?.Identity?.Name;
			if (string.IsNullOrEmpty(userName))
			{
				return Unauthorized(new { message = "User is not authenticated." });
			}

			manufactureDto.Data!.UpdatedBy = userName;
			var result = await _serviceProviders.ManufactureService.UpdateAsync(manufactureDto.Data);
			if (!result.Success)
			{
				return BadRequest(new { message = result.Error });
			}
			return Ok(result.Data);
		}
	}
}