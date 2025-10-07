using OEMEV.UserService.Application.Dtos;

namespace OEMEV.UserService.Application.Interfaces
{
	public interface IServiceCenterService
	{
		Task<Result<ServiceCenterDto>> GetServiceCenterByIdAsync(int id);
		Task<Result<List<ServiceCenterDto>>> GetAllServiceCentersAsync();
		Task<Result<ServiceCenterDto>> CreateServiceCenterAsync(ServiceCenterDto ServiceCenterDto);
		Task<Result<ServiceCenterDto>> UpdateServiceCenterAsync(ServiceCenterDto ServiceCenterDto);
		Task<Result<int>> DeleteServiceCenterAsync(long id);
	}
}
