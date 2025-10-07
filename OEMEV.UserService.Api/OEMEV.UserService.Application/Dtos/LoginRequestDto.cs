using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class LoginRequestDto
	{
		[Required]
		public string UserName { get; set; } = string.Empty;
		[Required]	
		public string Password { get; set; } = string.Empty;
	}
}
