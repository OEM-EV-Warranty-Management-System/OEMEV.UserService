using OEMEV.UserService.DAL.Interfaces;
using OEMEV.UserService.Data.Models;

namespace OEMEV.UserService.DAL.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(int Result, string? Error)> AddAsync(User user)
		{
			try
			{
				await _unitOfWork.GetRepository<User>().AddAsync(user);
				var result = await _unitOfWork.SaveAsync();
				return (result, null);
			}
			catch (Exception ex)
			{
				return (0, $"UserRepository.AddAsync error: {ex.Message}");
			}
		}

		public async Task<(IEnumerable<User> Users, string? Error)> GetAllAsync()
		{
			try
			{
				var users = await _unitOfWork
					.GetRepository<User>()
					.GetAllByPropertyAsync(null, includeProperties: "Role,ServiceCenter");

				return (users, null);
			}
			catch (Exception ex)
			{
				return (Enumerable.Empty<User>(), $"UserRepository.GetAllAsync error: {ex.Message}");
			}
		}

		public async Task<(User? User, string? Error)> GetByUserNameAsync(string userName)
		{
			try
			{
				var user = await _unitOfWork
					.GetRepository<User>()
					.GetByPropertyAsync(u => u.UserName == userName, includeProperties: "Role,ServiceCenter");

				return (user, null);
			}
			catch (Exception ex)
			{
				return (null, $"UserRepository.GetByUserNameAsync error: {ex.Message}");
			}
		}

		public async Task<(int Result, string? Error)> UpdateAsync(User user)
		{
			try
			{
				await _unitOfWork.GetRepository<User>().UpdateAsync(user);
				var result = await _unitOfWork.SaveAsync();
				
				return (result, null);
			}
			catch (Exception ex)
			{
				return (0, $"UserRepository.UpdateAsync error: {ex.Message}");
			}
		}
	}
}
