using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Application.Mappers
{
	public static class ManufactureMappers
	{
		public static ManufactureDto ToDto(Manufacture Manufacture)
		{
			return new ManufactureDto
			{
				Id = Manufacture.Id,
				Name = Manufacture.Name,
				Address = Manufacture.Address,
				Country = Manufacture.Country,
				Website = Manufacture.Website,
				ContactPhone = Manufacture.ContactPhone,
				ContactEmail = Manufacture.ContactEmail,
				IsActive = Manufacture.IsActive,
				CreatedAt = Manufacture.CreatedAt,
				UpdatedAt = Manufacture.UpdatedAt,
				CreatedBy = Manufacture.CreatedBy,
				UpdatedBy = Manufacture.UpdatedBy
			};
		}

		public static Manufacture ToEntity(ManufactureDto ManufactureDto)
		{

			var entity = new Manufacture
			{
				Name = ManufactureDto.Name,
				Address = ManufactureDto.Address,
				Country = ManufactureDto.Country,
				Website = ManufactureDto.Website,
				ContactPhone = ManufactureDto.ContactPhone,
				ContactEmail = ManufactureDto.ContactEmail,
				IsActive = ManufactureDto.IsActive,
				CreatedAt = ManufactureDto.CreatedAt,
				UpdatedAt = ManufactureDto.UpdatedAt,
				CreatedBy = ManufactureDto.CreatedBy,
				UpdatedBy = ManufactureDto.UpdatedBy
			};

			if (ManufactureDto.Id.HasValue && ManufactureDto.Id.Value > 0)
				entity.Id = ManufactureDto.Id.Value;

			return entity;
		}
	}
}
