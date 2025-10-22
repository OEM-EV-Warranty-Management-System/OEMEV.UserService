using OEMEV.UserService.Application.Dtos;

namespace OEMEV.UserService.Application.Interfaces
{
	public interface IServiceCenterService
	{
		Task<Result<List<ServiceCenterDto>>> GetAllAsync();
		Task<Result<ServiceCenterDto>> GetByIdAsync(long id);
		Task<Result<ServiceCenterDto>> CreateAsync(ServiceCenterDto serviceCenterDto);
		Task<Result<ServiceCenterDto>> UpdateAsync(ServiceCenterDto serviceCenterDto);
		Task<Result<int>> DeleteAsync(long id, string updatedBy);
	}
}