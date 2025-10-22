using OEMEV.UserService.Domain.Models;
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

		public async Task<(ServiceCenter? ServiceCenter, string? Error)> CreateAsync(ServiceCenter serviceCenter)
		{
			try
			{
				await _unitOfWork.GetRepository<ServiceCenter>().AddAsync(serviceCenter);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					var item = await _unitOfWork.GetRepository<ServiceCenter>().GetByPropertyAsync(sc => sc.Id == serviceCenter.Id);
					return (item, null);
				}
				return (null, "ServiceCenterRepository.CreateAsync: Unable to create service center.");
			}
			catch (Exception ex)
			{
				return (null, $"ServiceCenterRepository.CreateAsync: {ex.Message}");
			}
		}

		public async Task<(IEnumerable<ServiceCenter> ServiceCenters, string? Error)> GetAllAsync()
		{
			try
			{
				var serviceCenters = await _unitOfWork.GetRepository<ServiceCenter>().GetAllByPropertyAsync();
				return (serviceCenters, null);
			}
			catch (Exception ex)
			{
				return (Enumerable.Empty<ServiceCenter>(), $"ServiceCenterRepository.GetAllAsync: {ex.Message}");
			}
		}

		public async Task<(ServiceCenter? ServiceCenter, string? Error)> GetByIdAsync(long id)
		{
			try
			{
				var serviceCenter = await _unitOfWork.GetRepository<ServiceCenter>().GetByPropertyAsync(sc => sc.Id == id);
				return (serviceCenter, null);
			}
			catch (Exception ex)
			{
				return (null, $"ServiceCenterRepository.GetByIdAsync: {ex.Message}");
			}
		}

		public async Task<(ServiceCenter? ServiceCenter, string? Error)> UpdateAsync(ServiceCenter serviceCenter)
		{
			try
			{
				await _unitOfWork.GetRepository<ServiceCenter>().UpdateAsync(serviceCenter);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					return (serviceCenter, null);
				}
				return (null, "ServiceCenterRepository.UpdateAsync: Unable to update service center.");
			}
			catch (Exception ex)
			{
				return (null, $"ServiceCenterRepository.UpdateAsync: {ex.Message}");
			}
		}
	}
}