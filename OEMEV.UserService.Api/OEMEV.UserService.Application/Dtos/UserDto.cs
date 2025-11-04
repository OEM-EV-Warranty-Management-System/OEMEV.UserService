using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class UserDto
	{
		[SwaggerSchema(ReadOnly = true)]
		public Guid? Id { get; set; }
		public string? UserName { get; set; }
		public string FullName { get; set; } = null!;
		[Phone]
		public string? PhoneNumber { get; set; }
		[EmailAddress]
		public string? Email { get; set; }
		[Required]
		public long RoleId { get; set; }
		public long? ServiceCenterId { get; set; }
		public long? ManufacturerId { get; set; }
		public bool IsActive { get; set; }
	}
}
