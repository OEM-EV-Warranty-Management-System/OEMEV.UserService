using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Infrastructure.Interfaces
{
	public interface IServiceCenterRepository
	{
		Task<(IEnumerable<ServiceCenter> ServiceCenters, string? Error)> GetAllServiceCentersAsync();
		Task<(ServiceCenter? ServiceCenter, string? Error)> GetServiceCenterByIdAsync(long id);
		Task<(int Result, string? Error)> AddServiceCenterAsync(ServiceCenter serviceCenter);
		Task<(int Result, string? Error)> UpdateServiceCenterAsync(ServiceCenter serviceCenter);
		Task<(int Result, string? Error)> DeleteServiceCenterAsync(long id);
	}
}
