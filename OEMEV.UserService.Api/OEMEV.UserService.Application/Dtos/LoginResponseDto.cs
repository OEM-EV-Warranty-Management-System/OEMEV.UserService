namespace OEMEV.UserService.Application.Dtos
{
	public class LoginResponseDto
	{
		public string? AccessToken { get; set; } 
		public string? RefreshToken { get; set; } 
		public string? Error { get; set; } 
	}
}
