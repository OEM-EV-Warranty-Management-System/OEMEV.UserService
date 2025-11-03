using System.ComponentModel.DataAnnotations;

public class ResetPasswordRequestDto
{
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string Otp { get; set; } = string.Empty;

	[Required]
	[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
	public string NewPassword { get; set; } = string.Empty;
}
