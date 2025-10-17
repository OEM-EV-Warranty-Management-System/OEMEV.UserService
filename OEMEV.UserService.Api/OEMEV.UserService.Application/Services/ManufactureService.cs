using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Application.Mappers;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Application.Services
{
	public class ManufactureService : IManufactureService
	{
		private readonly IManufactureRepository _repo;
		public ManufactureService(IManufactureRepository repo) => _repo = repo;
		public async Task<Result<ManufactureDto>> CreateAsync(ManufactureDto manufactureDto)
		{
			try
			{
				var manufacture = ManufactureMappers.ToEntity(manufactureDto);
				manufacture.CreatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.CreateAsync(manufacture);
				if (error != null)
					return Result<ManufactureDto>.Fail(error);
				return Result<ManufactureDto>.Ok(ManufactureMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<ManufactureDto>.Fail($"ManufactureService.CreateAsync: {ex.Message}");
			}
		}

		public async Task<Result<int>> DeleteAsync(long id)
		{
			try
			{
				var (manufactureCheck, errorMsg) = await _repo.GetByIdAsync(id);
				if (errorMsg != null)
					return Result<int>.Fail(errorMsg);
				if (manufactureCheck == null)
					return Result<int>.Fail("Manufacture not found!");
				var (result, error) = await _repo.DeleteAsync(id);
				if (error != null)
					return Result<int>.Fail(error);
				return Result<int>.Ok(result);
			}
			catch (Exception ex)
			{
				return Result<int>.Fail($"ManufactureService.DeleteAsync: {ex.Message}");
			}
		}

		public async Task<Result<List<ManufactureDto>>> GetAllAsync()
		{
			try
			{
				var (manufactures, error) = await _repo.GetAllAsync();
				if (error != null)
					return Result<List<ManufactureDto>>.Fail(error);
				var manufactureDtos = manufactures.Select(ManufactureMappers.ToDto).ToList();
				return Result<List<ManufactureDto>>.Ok(manufactureDtos);
			} catch (Exception ex)
			{
				return Result<List<ManufactureDto>>.Fail($"ManufactureService.GetAllAsync: {ex.Message}");
			}
		}

		public async Task<Result<ManufactureDto>> GetByIdAsync(long id)
		{
			try
			{
				var (manufacture, error) = await _repo.GetByIdAsync(id);
				if (error != null)
					return Result<ManufactureDto>.Fail(error);
				if (manufacture == null)
					return Result<ManufactureDto>.Fail("Manufacture not found!");
				return Result<ManufactureDto>.Ok(ManufactureMappers.ToDto(manufacture));
			} catch (Exception ex)
			{
				return Result<ManufactureDto>.Fail($"ManufactureService.GetByIdAsync: {ex.Message}");
			}
		}

		public async Task<Result<ManufactureDto>> UpdateAsync(ManufactureDto manufactureDto)
		{
			try
			{
				var (manufactureCheck, errorMsg) = await _repo.GetByIdAsync(manufactureDto.Id.Value);
				if (errorMsg != null)
					return Result<ManufactureDto>.Fail(errorMsg);
				if (manufactureCheck == null)
					return Result<ManufactureDto>.Fail("Manufacture not found!");
				var manufacture = ManufactureMappers.ToEntity(manufactureDto);
				manufacture.UpdatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.UpdateAsync(manufacture);
				if (error != null)
					return Result<ManufactureDto>.Fail(error);
				return Result<ManufactureDto>.Ok(ManufactureMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<ManufactureDto>.Fail($"ManufactureService.UpdateAsync: {ex.Message}");
			}
		}
	}
}
