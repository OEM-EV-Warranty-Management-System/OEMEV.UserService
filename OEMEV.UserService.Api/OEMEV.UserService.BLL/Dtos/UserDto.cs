using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.BLL.Dtos
{
	public class UserDto
	{
		[SwaggerSchema(ReadOnly = true)]
		public Guid? Id { get; set; }
		[Required(ErrorMessage = "User name is required!")]
		public string UserName { get; set; } = null!;
		[Required(ErrorMessage = "Full name is required!")]
		public string FullName { get; set; } = null!;
		[Phone]
		public string? PhoneNumber { get; set; }
		[EmailAddress]
		public string? Email { get; set; }
		[Required]
		public long RoleId { get; set; }
		public long? ServiceCenterId { get; set; }
	}
}
