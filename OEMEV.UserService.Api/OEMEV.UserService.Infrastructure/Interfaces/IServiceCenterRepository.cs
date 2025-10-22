using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Infrastructure.Interfaces
{
	public interface IServiceCenterRepository
	{
		Task<(IEnumerable<ServiceCenter> ServiceCenters, string? Error)> GetAllAsync();
		Task<(ServiceCenter? ServiceCenter, string? Error)> GetByIdAsync(long id);
		Task<(ServiceCenter? ServiceCenter, string? Error)> CreateAsync(ServiceCenter serviceCenter);
		Task<(ServiceCenter? ServiceCenter, string? Error)> UpdateAsync(ServiceCenter serviceCenter);
	}
}