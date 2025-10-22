using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class ServiceCenterDto
	{
		[SwaggerSchema(ReadOnly = true)]
		public long? Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Address is required.")]
		public string Address { get; set; } = null!;

		[Phone]
		public string? ContactPhone { get; set; }

		[EmailAddress]
		public string? ContactEmail { get; set; }

		public bool IsActive { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public DateTime CreatedAt { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public DateTime? UpdatedAt { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public string CreatedBy { get; set; } = string.Empty;

		[SwaggerSchema(ReadOnly = true)]
		public string? UpdatedBy { get; set; }
	}
}