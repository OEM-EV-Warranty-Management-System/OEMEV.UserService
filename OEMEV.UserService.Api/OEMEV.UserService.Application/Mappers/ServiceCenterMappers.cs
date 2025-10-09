using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Application.Mappers
{
	public static class ServiceCenterMappers
	{
		public static ServiceCenterDto ToDto(ServiceCenter serviceCenter)
		{
			return new ServiceCenterDto
			{
				Id = serviceCenter.Id,
				Name = serviceCenter.Name,
				Address = serviceCenter.Address,
				ContactPhone = serviceCenter.ContactPhone,
				ContactEmail = serviceCenter.ContactEmail,
				IsActive = serviceCenter.IsActive,
				CreatedAt = serviceCenter.CreatedAt,
				UpdatedAt = serviceCenter.UpdatedAt ?? null,
			};
		}

		public static ServiceCenter ToEntity(ServiceCenterDto serviceCenterDto)
		{

			var entity = new ServiceCenter
			{
				Name = serviceCenterDto.Name,
				Address = serviceCenterDto.Address,
				ContactPhone = serviceCenterDto.ContactPhone,
				ContactEmail = serviceCenterDto.ContactEmail,
				IsActive = serviceCenterDto.IsActive,
				CreatedAt = serviceCenterDto.CreatedAt,
				UpdatedAt = serviceCenterDto.UpdatedAt
			};

			if (serviceCenterDto.Id.HasValue && serviceCenterDto.Id.Value > 0)
				entity.Id = serviceCenterDto.Id.Value;

			
				return entity;
		}
	}
}
