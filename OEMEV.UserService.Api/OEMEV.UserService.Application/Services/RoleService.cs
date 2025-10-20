using OEMEV.UserService.Application.Dtos;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Application.Mappers;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Application.Services
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _repo;
		public RoleService(IRoleRepository repo) => _repo = repo;

		public async Task<Result<RoleDto>> CreateAsync(RoleDto roleDto)
		{
			try
			{
				var role = RoleMappers.ToEntity(roleDto);
				role.CreatedAt = DateTime.UtcNow;
				var (result, error) = await _repo.CreateAsync(role);
				if (error != null || result == null)
					return Result<RoleDto>.Fail(error ?? "Failed to create role.");
				return Result<RoleDto>.Ok(RoleMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<RoleDto>.Fail($"RoleService.CreateAsync: {ex.Message}");
			}
		}

		public async Task<Result<int>> DeleteAsync(long id, string updatedBy)
		{
			try
			{
				var (role, errorMsg) = await _repo.GetByIdAsync(id);
				if (errorMsg != null)
					return Result<int>.Fail(errorMsg);
				if (role == null)
					return Result<int>.Fail("Role not found!");

				role.IsActive = false; 
				role.UpdatedAt = DateTime.UtcNow;
				role.UpdatedBy = updatedBy;

				var (updatedRoleResult, updateError) = await _repo.UpdateAsync(role);
				if (updateError != null)
					return Result<int>.Fail(updateError);

				return Result<int>.Ok(updatedRoleResult != null ? 1 : 0);
			}
			catch (Exception ex)
			{
				return Result<int>.Fail($"RoleService.DeleteAsync: {ex.Message}");
			}
		}

		public async Task<Result<List<RoleDto>>> GetAllAsync()
		{
			try
			{
				var (roles, error) = await _repo.GetAllAsync();
				if (error != null)
					return Result<List<RoleDto>>.Fail(error);
				var roleDtos = roles.Select(RoleMappers.ToDto).ToList();
				return Result<List<RoleDto>>.Ok(roleDtos);
			}
			catch (Exception ex)
			{
				return Result<List<RoleDto>>.Fail($"RoleService.GetAllAsync: {ex.Message}");
			}
		}

		public async Task<Result<RoleDto>> GetByIdAsync(long id)
		{
			try
			{
				var (role, error) = await _repo.GetByIdAsync(id);
				if (error != null)
					return Result<RoleDto>.Fail(error);
				if (role == null)
					return Result<RoleDto>.Fail("Role not found!");
				return Result<RoleDto>.Ok(RoleMappers.ToDto(role));
			}
			catch (Exception ex)
			{
				return Result<RoleDto>.Fail($"RoleService.GetByIdAsync: {ex.Message}");
			}
		}

		public async Task<Result<RoleDto>> UpdateAsync(RoleDto roleDto)
		{
			try
			{
				var (roleCheck, errorMsg) = await _repo.GetByIdAsync(roleDto.Id.Value);
				if (errorMsg != null)
					return Result<RoleDto>.Fail(errorMsg);
				if (roleCheck == null)
					return Result<RoleDto>.Fail("Role not found!");

				var role = RoleMappers.ToEntity(roleDto);
				role.CreatedAt = roleCheck.CreatedAt;
				role.CreatedBy = roleCheck.CreatedBy;
				role.UpdatedAt = DateTime.UtcNow;

				var (result, error) = await _repo.UpdateAsync(role);
				if (error != null || result == null)
					return Result<RoleDto>.Fail(error ?? "Failed to update role.");
				return Result<RoleDto>.Ok(RoleMappers.ToDto(result));
			}
			catch (Exception ex)
			{
				return Result<RoleDto>.Fail($"RoleService.UpdateAsync: {ex.Message}");
			}
		}
	}
}