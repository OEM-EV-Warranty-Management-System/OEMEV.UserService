using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Application.Mappers
{
	public class UserMappers
	{
		public static UserDto ToDto(User user)
		{
			return new UserDto()
			{
				Id = user.Id,
				UserName = user.UserName ?? string.Empty,
				FullName = user.FullName ?? string.Empty,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				RoleId = user.RoleId,
				RoleName = user.Role?.Name ?? string.Empty,
				ServiceCenterId = user.ServiceCenterId ?? null,
				ServiceCenterName = user.ServiceCenter?.Name ?? string.Empty,
				ManufacturerId = user.ManufacturerId ?? null,
				ManufacturerName = user.Manufacturer?.Name ?? string.Empty,
				IsActive = user.IsActive,
				CreatedAt = user.CreatedAt,
			};
		}

		public static User ToEntity(UserDto dto)
		{
			var entity = new User()
			{
				UserName = dto.UserName ?? string.Empty,
				FullName = dto.FullName ?? string.Empty,
				PhoneNumber = dto.PhoneNumber,
				Email = dto.Email,
				RoleId = dto.RoleId,
				ServiceCenterId = dto.ServiceCenterId ?? null,
				ManufacturerId = dto.ManufacturerId ?? null,
				IsActive = dto.IsActive,
			};

			if (dto.Id != null) entity.Id = dto.Id.Value;
			if (dto.CreatedAt != null) entity.CreatedAt = dto.CreatedAt.Value;
			return entity;
		}
	}
}
