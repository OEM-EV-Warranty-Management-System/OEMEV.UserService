using OEMEV.UserService.Application.Dtos;

namespace OEMEV.UserService.Application.Interfaces
{
	public interface IManufactureService
	{
		Task<Result<List<ManufactureDto>>> GetAllAsync();
		Task<Result<ManufactureDto>> GetByIdAsync(long id);
		Task<Result<ManufactureDto>> CreateAsync(ManufactureDto manufactureDto);
		Task<Result<ManufactureDto>> UpdateAsync(ManufactureDto manufactureDto);
		Task<Result<int>> DeleteAsync(long id);
	}
}
