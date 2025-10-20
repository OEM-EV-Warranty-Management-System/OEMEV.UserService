using OEMEV.UserService.Domain.Models;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Infrastructure.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		private readonly IUnitOfWork _unitOfWork;
		public RoleRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;	
		}

		public async Task<(Role? Role, string? Error)> CreateAsync(Role role)
		{
			try
			{
				await _unitOfWork.GetRepository<Role>().AddAsync(role);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					var item = await _unitOfWork.GetRepository<Role>().GetByPropertyAsync(r => r.Id == role.Id);
					return (item, null);
				}
				return (null, "RoleRepository.CreateAsync: Unable to create role.");
			}
			catch (Exception ex)
			{
				return (null, $"RoleRepository.CreateAsync: {ex.Message}");
			}
		}

		public async Task<(IEnumerable<Role> Role, string? Error)> GetAllAsync()
		{
			try
			{
				var roles = await _unitOfWork.GetRepository<Role>().GetAllByPropertyAsync();
				return (roles, null);
			}
			catch (Exception ex)
			{
				return (Enumerable.Empty<Role>(), $"RoleRepository.GetAllAsync: {ex.Message}");
			}
		}

		public async Task<(Role? Role, string? Error)> GetByIdAsync(long id)
		{
			try
			{
				var role = await _unitOfWork.GetRepository<Role>().GetByPropertyAsync(r => r.Id == id);
				return (role, null);
			}
			catch (Exception ex)
			{
				return (null, $"RoleRepository.GetByIdAsync: {ex.Message}");
			}
		}

		public async Task<(Role? Role, string? Error)> UpdateAsync(Role role)
		{
			try
			{
				await _unitOfWork.GetRepository<Role>().UpdateAsync(role);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					return (role, null);
				}
				return (null, "RoleRepository.UpdateAsync: Unable to update role.");
			} catch (Exception ex)
			{
				return (null, $"RoleRepository.UpdateAsync: {ex.Message}");
			}
		}
	}
}
