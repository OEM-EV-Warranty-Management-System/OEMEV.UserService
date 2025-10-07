using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Domain;

namespace OEMEV.UserService.Application.Mappers
{
	public class UserMappers
	{
		public static UserDto ToDto(User user)
		{
			return new UserDto()
			{
				Id = user.Id,
				UserName = user.UserName,
				FullName = user.FullName ?? string.Empty,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				RoleId = user.RoleId,
				ServiceCenterId = user.ServiceCenterId ?? null
			};
		}

		public static User ToEntity(UserDto dto)
		{
			return new User()
			{
				Id = dto.Id ?? Guid.Empty,
				UserName = dto.UserName,
				FullName = dto.FullName ?? string.Empty,
				PhoneNumber = dto.PhoneNumber,
				Email = dto.Email,
				RoleId = dto.RoleId,
				ServiceCenterId = dto.ServiceCenterId ?? null
			};
		}
	}
}
