using OEMEV.UserService.Domain.Models;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<(User? User, string? Error)> AddAsync(User user)
		{
			try
			{
				await _unitOfWork.GetRepository<User>().AddAsync(user);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					var item = await _unitOfWork.GetRepository<User>()
						.GetByPropertyAsync(u => u.Id == user.Id, includeProperties: "Role,ServiceCenter");
					return (item, null);
				}
				return (null, "UserRepository.AddAsync: Unable to create user.");
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null && ex.InnerException.Message.Contains("users_user_name_key"))
				{
					return (null, "A user with this username already exists.");
				}
				return (null, $"UserRepository.AddAsync error: {ex.Message}");
			}
		}

		public async Task<(IEnumerable<User> Users, string? Error)> GetAllAsync()
		{
			try
			{
				var users = await _unitOfWork
					.GetRepository<User>()
					.GetAllByPropertyAsync(u => u.IsActive, includeProperties: "Role,ServiceCenter");

				return (users, null);
			}
			catch (Exception ex)
			{
				return (Enumerable.Empty<User>(), $"UserRepository.GetAllAsync error: {ex.Message}");
			}
		}

		public async Task<(User? User, string? Error)> GetByIdAsync(Guid id)
		{
			try
			{
				var user = await _unitOfWork
					.GetRepository<User>()
					.GetByPropertyAsync(u => u.Id == id, includeProperties: "Role,ServiceCenter");

				return (user, null);
			}
			catch (Exception ex)
			{
				return (null, $"UserRepository.GetByIdAsync error: {ex.Message}");
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
		
		public async Task<(User? User, string? Error)> GetByEmailAsync(string email)
		{
			try
			{
				var user = await _unitOfWork
					.GetRepository<User>()
					.GetByPropertyAsync(u => u.Email == email, includeProperties: "Role,ServiceCenter");

				return (user, null);
			}
			catch (Exception ex)
			{
				return (null, $"UserRepository.GetByEmailAsync error: {ex.Message}");
			}
		}

		public async Task<(User? User, string? Error)> UpdateAsync(User user)
		{
			try
			{
				await _unitOfWork.GetRepository<User>().UpdateAsync(user);
				var result = await _unitOfWork.SaveAsync();

				if (result > 0)
				{
					var item = await _unitOfWork.GetRepository<User>()
						.GetByPropertyAsync(u => u.Id == user.Id, includeProperties: "Role,ServiceCenter");
					return (item, null);
				}
				return (null, "UserRepository.UpdateAsync: Unable to update user.");
			}
			catch (Exception ex)
			{
				return (null, $"UserRepository.UpdateAsync error: {ex.Message}");
			}
		}
	}
}