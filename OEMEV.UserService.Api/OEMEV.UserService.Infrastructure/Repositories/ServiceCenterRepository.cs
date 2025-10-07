using OEMEV.UserService.Domain;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Infrastructure.Repositories
{
	public class ServiceCenterRepository : IServiceCenterRepository
	{
		private readonly IUnitOfWork _unitOfWork;
		public ServiceCenterRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(int Result, string? Error)> AddServiceCenterAsync(ServiceCenter serviceCenter)
		{
			try
			{
				await _unitOfWork.GetRepository<ServiceCenter>().AddAsync(serviceCenter);
				var result = await _unitOfWork.SaveAsync();

				return (result, null);
			}
			catch (Exception ex)
			{
				return (0, $"ServiceCenterRepository.AddServiceCenterAsync: {ex.Message}");
			}
		}

		public async Task<(int Result, string? Error)> DeleteServiceCenterAsync(long id)
		{
			try
			{
				await _unitOfWork.GetRepository<ServiceCenter>().DeleteAsync(id);
				var result = await _unitOfWork.SaveAsync();

				return (result, null);
			}
			catch (Exception ex)
			{
				return (0, $"ServiceCenterRepository.DeleteServiceCenterAsync: {ex.Message}");
			}

		}

		public async Task<(IEnumerable<ServiceCenter> ServiceCenters, string? Error)> GetAllServiceCentersAsync()
		{
			try
			{
				var serviceCenters = await _unitOfWork.GetRepository<ServiceCenter>().GetAllByPropertyAsync();

				return (serviceCenters, null);
			}
			catch (Exception ex)
			{
				return (Enumerable.Empty<ServiceCenter>(), $"ServiceCenterRepository.GetAllServiceCentersAsync: {ex.Message}");
			}
		}

		public async Task<(ServiceCenter? ServiceCenter, string? Error)> GetServiceCenterByIdAsync(int id)
		{
			try
			{
				var serviceCenter = await _unitOfWork.GetRepository<ServiceCenter>().GetByPropertyAsync(sc => sc.Id == id);

				return (serviceCenter, null);
			}
			catch (Exception ex)
			{
				return (null, $"ServiceCenterRepository.GetServiceCenterByIdAsync: {ex.Message}");
			}
		}

		public async Task<(int Result, string? Error)> UpdateServiceCenterAsync(ServiceCenter serviceCenter)
		{
			try
			{
				await _unitOfWork.GetRepository<ServiceCenter>().UpdateAsync(serviceCenter);
				var result = await _unitOfWork.SaveAsync();

				return (result, null);
			}
			catch (Exception ex)
			{
				return (0, $"ServiceCenterRepository.UpdateServiceCenterAsync: {ex.Message}");
			}
		}
	}
}
