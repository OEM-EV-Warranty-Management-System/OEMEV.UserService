using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Application.Mappers
{
	public static class RoleMappers
	{
		public static RoleDto ToDto(Role role)
		{
			return new RoleDto
			{
				Id = role.Id,
				Name = role.Name,
				IsActive = role.IsActive,
				CreatedAt = role.CreatedAt,
				UpdatedAt = role.UpdatedAt,
				CreatedBy = role.CreatedBy,
				UpdatedBy = role.UpdatedBy
			};
		}

		public static Role ToEntity(RoleDto roleDto)
		{
			var entity = new Role
			{
				Name = roleDto.Name,
				IsActive = roleDto.IsActive,
				CreatedAt = roleDto.CreatedAt,
				UpdatedAt = roleDto.UpdatedAt,
				CreatedBy = roleDto.CreatedBy,
				UpdatedBy = roleDto.UpdatedBy
			};

			if (roleDto.Id.HasValue && roleDto.Id.Value > 0)
				entity.Id = roleDto.Id.Value;

			return entity;
		}
	}
}