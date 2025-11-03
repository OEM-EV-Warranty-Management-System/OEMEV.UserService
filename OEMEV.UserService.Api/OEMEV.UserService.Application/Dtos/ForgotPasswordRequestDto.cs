using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class ForgotPasswordRequestDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}
}