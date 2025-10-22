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

		public async Task<Result<ServiceCenterDto>> CreateAsync(ServiceCenterDto serviceCenterDto)
		{
			try
			{
				var serviceCenter = ServiceCenterMappers.ToEntity(serviceCenterDto);
				serviceCenter.CreatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.CreateAsync(serviceCenter);
				if (error != null || result == null)
					return Result<ServiceCenterDto>.Fail(error ?? "Failed to create service center.");
				return Result<ServiceCenterDto>.Ok(ServiceCenterMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<ServiceCenterDto>.Fail($"ServiceCenterService.CreateAsync: {ex.Message}");
			}
		}

		public async Task<Result<int>> DeleteAsync(long id, string updatedBy)
		{
			try
			{
				var (serviceCenter, errorMsg) = await _repo.GetByIdAsync(id);
				if (errorMsg != null)
					return Result<int>.Fail(errorMsg);
				if (serviceCenter == null)
					return Result<int>.Fail("Service center not found!");

				serviceCenter.IsActive = false;
				serviceCenter.UpdatedAt = DateTime.UtcNow;
				serviceCenter.UpdatedBy = updatedBy;

				var (updatedServiceCenterResult, updateError) = await _repo.UpdateAsync(serviceCenter);
				if (updateError != null)
					return Result<int>.Fail(updateError);

				return Result<int>.Ok(updatedServiceCenterResult != null ? 1 : 0);
			}
			catch (Exception ex)
			{
				return Result<int>.Fail($"ServiceCenterService.DeleteAsync: {ex.Message}");
			}
		}

		public async Task<Result<List<ServiceCenterDto>>> GetAllAsync()
		{
			try
			{
				var (serviceCenters, error) = await _repo.GetAllAsync();
				if (error != null)
					return Result<List<ServiceCenterDto>>.Fail(error);
				var serviceCenterDtos = serviceCenters.Select(ServiceCenterMappers.ToDto).ToList();
				return Result<List<ServiceCenterDto>>.Ok(serviceCenterDtos);
			}
			catch (Exception ex)
			{
				return Result<List<ServiceCenterDto>>.Fail($"ServiceCenterService.GetAllAsync: {ex.Message}");
			}
		}

		public async Task<Result<ServiceCenterDto>> GetByIdAsync(long id)
		{
			try
			{
				var (serviceCenter, error) = await _repo.GetByIdAsync(id);
				if (error != null)
					return Result<ServiceCenterDto>.Fail(error);
				if (serviceCenter == null)
					return Result<ServiceCenterDto>.Fail("Service center not found!");
				return Result<ServiceCenterDto>.Ok(ServiceCenterMappers.ToDto(serviceCenter));
			}
			catch (Exception ex)
			{
				return Result<ServiceCenterDto>.Fail($"ServiceCenterService.GetByIdAsync: {ex.Message}");
			}
		}

		public async Task<Result<ServiceCenterDto>> UpdateAsync(ServiceCenterDto serviceCenterDto)
		{
			try
			{
				var (serviceCenterCheck, errorMsg) = await _repo.GetByIdAsync(serviceCenterDto.Id.Value);
				if (errorMsg != null)
					return Result<ServiceCenterDto>.Fail(errorMsg);
				if (serviceCenterCheck == null)
					return Result<ServiceCenterDto>.Fail("Service center not found!");

				var serviceCenter = ServiceCenterMappers.ToEntity(serviceCenterDto);
				serviceCenter.CreatedAt = serviceCenterCheck.CreatedAt;
				serviceCenter.CreatedBy = serviceCenterCheck.CreatedBy;
				serviceCenter.UpdatedAt = DateTime.UtcNow;

				var (result, error) = await _repo.UpdateAsync(serviceCenter);
				if (error != null || result == null)
					return Result<ServiceCenterDto>.Fail(error ?? "Failed to update service center.");
				return Result<ServiceCenterDto>.Ok(ServiceCenterMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<ServiceCenterDto>.Fail($"ServiceCenterService.UpdateAsync: {ex.Message}");
			}
		}
	}
}