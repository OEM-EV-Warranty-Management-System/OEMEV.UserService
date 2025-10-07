using Microsoft.AspNetCore.Mvc;
using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceCenterController : ControllerBase
	{
		private readonly IServiceProviders _serviceProviders;
		public ServiceCenterController(IServiceProviders serviceProviders)
		{
			_serviceProviders = serviceProviders;
		}
		[HttpGet("get-all")]
		public async Task<IActionResult> GetAllServiceCenter()
		{
			var result = await _serviceProviders.ServiceCenterService.GetAllServiceCentersAsync();
			if (!result.Success) return BadRequest(new { message = result.Error });
			return Ok(result.Data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			var result = await _serviceProviders.ServiceCenterService.GetServiceCenterByIdAsync(id);
			if (!result.Success) return BadRequest(new { message = result.Error });
			if(result.Data == null) return NotFound(new { message = "Service center can not found." });
			return Ok(result.Data);
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateServiceCenter([FromBody] ServiceCenterDto dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			var result = await _serviceProviders.ServiceCenterService.CreateServiceCenterAsync(dto);
			if (!result.Success) return BadRequest(new { message = result.Error });
			return CreatedAtAction(nameof(GetById), new { id = result.Data }, new { result = "Service center created successfully" });
		}

		[HttpPut("update/{id}")]
		public async Task<IActionResult> UpdateServiceCenter([FromRoute] long id,[FromBody] ServiceCenterDto dto)
		{
			if (id == null) return BadRequest(new { message = "Id can not null" });
			if (!ModelState.IsValid) return BadRequest(ModelState);
			dto.Id = id;
			var result = await _serviceProviders.ServiceCenterService.UpdateServiceCenterAsync(dto);
			if (!result.Success) return BadRequest(new { message = result.Error });
			return Ok(new { result = "Service center updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute]long id)
		{
			var result = await _serviceProviders.ServiceCenterService.DeleteServiceCenterAsync(id);
			if (!result.Success) return BadRequest(new { message = result.Error });
			return Ok(new { result = "Service center deleted successfully" });
		}
	}
}
