using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class ManufactureDto
	{
		[SwaggerSchema(ReadOnly = true)]
		public long? Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Country is required.")]
		public string Country { get; set; } = null!;

		public string? Address { get; set; }

		[Phone]
		public string? ContactPhone { get; set; }
		[EmailAddress]

		public string? ContactEmail { get; set; }

		public string? Website { get; set; }

		public bool IsActive { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public DateTime CreatedAt { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public DateTime? UpdatedAt { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public string CreatedBy { get; set; } = null!;
		[SwaggerSchema(ReadOnly = true)]
		public string? UpdatedBy { get; set; }
	}
}
