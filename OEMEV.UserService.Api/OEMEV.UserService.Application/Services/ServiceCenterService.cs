using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Application.Mappers;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Application.Services
{
	public class ServiceCenterService : IServiceCenterService
	{
		private readonly IServiceCenterRepository _repo;
		public ServiceCenterService(IServiceCenterRepository repo)
		{
			_repo = repo;
		}

		public async Task<Result<ServiceCenterDto>> CreateServiceCenterAsync(ServiceCenterDto ServiceCenterDto)
		{
			try
			{
				var serviceCenter = ServiceCenterMappers.ToEntity(ServiceCenterDto);
				serviceCenter.CreatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.AddServiceCenterAsync(serviceCenter);
				if (error != null)
					return Result<ServiceCenterDto>.Fail(error);
				if (result <= 0)
					return Result<ServiceCenterDto>.Fail("Failed to create Service Center.");
				var (createdServiceCenter, errorMsg) = await _repo.GetServiceCenterByIdAsync(serviceCenter.Id);
				if (errorMsg != null || createdServiceCenter == null)
					return Result<ServiceCenterDto>.Fail("Failed to retrieve created Service Center.");
				ServiceCenterDto = ServiceCenterMappers.ToDto(createdServiceCenter);
				return Result<ServiceCenterDto>.Ok(ServiceCenterDto);
			}
			catch (Exception ex)
			{
				return Result<ServiceCenterDto>.Fail($"ServiceCenterService.CreateServiceCenterAsync: {ex.Message}");
			}
		}

		public async Task<Result<int>> HardDeleteServiceCenterAsync(long id)
		{
			try
			{
				var (deleteResult, deleteError) = await _repo.DeleteServiceCenterAsync(id);
				if (deleteError != null)
					return Result<int>.Fail(deleteError);
				if (deleteResult <= 0)
					return Result<int>.Fail("Failed to delete Service Center.");
				return Result<int>.Ok(deleteResult);
			}
			catch (Exception ex)
			{
				return Result<int>.Fail($"ServiceCenterService.DeleteServiceCenterAsync: {ex.Message}");
			}

		}

		public async Task<Result<List<ServiceCenterDto>>> GetAllServiceCentersAsync()
		{
			try
			{
				var (serviceCenters, error) = await _repo.GetAllServiceCentersAsync();
				if (error != null)
					return Result<List<ServiceCenterDto>>.Fail(error);
				var serviceCenterDtos = serviceCenters.Select(ServiceCenterMappers.ToDto).ToList();
				return Result<List<ServiceCenterDto>>.Ok(serviceCenterDtos);
			}
			catch (Exception ex)
			{
				return Result<List<ServiceCenterDto>>.Fail($"ServiceCenterService.GetAllServiceCentersAsync: {ex.Message}");
			}
		}

		public async Task<Result<ServiceCenterDto>> GetServiceCenterByIdAsync(long id)
		{
			try
			{
				var (serviceCenter, error) = await _repo.GetServiceCenterByIdAsync(id);
				if (serviceCenter == null)
					return Result<ServiceCenterDto>.Fail("Service Center not found.");
				var serviceCenterDto = ServiceCenterMappers.ToDto(serviceCenter);
				return Result<ServiceCenterDto>.Ok(serviceCenterDto);
			}
			catch (Exception ex)
			{
				return Result<ServiceCenterDto>.Fail($"ServiceCenterService.GetServiceCenterByIdAsync: {ex.Message}");
			}
		}

		public async Task<Result<ServiceCenterDto>> UpdateServiceCenterAsync(ServiceCenterDto ServiceCenterDto)
		{
			try
			{
				var serviceCenter = ServiceCenterMappers.ToEntity(ServiceCenterDto);
				serviceCenter.UpdatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.UpdateServiceCenterAsync(serviceCenter);
				if (error != null)
					return Result<ServiceCenterDto>.Fail(error);
				if (result <= 0)
					return Result<ServiceCenterDto>.Fail("Failed to update Service Center.");
				return Result<ServiceCenterDto>.Ok(ServiceCenterDto);
			}
			catch (Exception ex)
			{
				return Result<ServiceCenterDto>.Fail($"ServiceCenterService.UpdateServiceCenterAsync: {ex.Message}");
			}
		}

		public async Task<Result<int>> SetStatusAsync(ServiceCenterDto ServiceCenterDto)
		{
			try
			{
				var serviceCenter = ServiceCenterMappers.ToEntity(ServiceCenterDto);
				serviceCenter.UpdatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.UpdateServiceCenterAsync(serviceCenter);
				if (error != null)
					return Result<int>.Fail(error);
				if (result <= 0)
					return Result<int>.Fail("Failed to soft delete Service Center.");
				return Result<int>.Ok(result);
			} catch (Exception ex)
			{
				return Result<int>.Fail($"ServiceCenterService.SoftDeleteServiceCenterAsync: {ex.Message}");
			}
		}
	}
}