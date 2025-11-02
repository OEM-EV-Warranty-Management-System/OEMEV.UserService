using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OEMEV.UserService.Application.Dtos
{
	public class RefreshTokenRequestDto
	{
		[Required]
		public string AccessToken { get; set; } = string.Empty;

		[Required]
		public string RefreshToken { get; set; } = string.Empty;
	}
}