using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class RoleDto
	{
		[SwaggerSchema(ReadOnly = true)]
		public long? Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; } = null!;

		public bool IsActive { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public DateTime CreatedAt { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public DateTime? UpdatedAt { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public string? CreatedBy { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public string? UpdatedBy { get; set; }
	}
}