using OEMEV.UserService.Domain.Models;
using OEMEV.UserService.Infrastructure.Interfaces;

namespace OEMEV.UserService.Infrastructure.Repositories
{
	public class ManufactureRepository : IManufactureRepository
	{
		private readonly IUnitOfWork _unitOfWork;

		public ManufactureRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<(Manufacture? Manufacture, string? Error)> CreateAsync(Manufacture manufacture)
		{
			try
			{
				await _unitOfWork.GetRepository<Manufacture>().AddAsync(manufacture);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					 var item = await _unitOfWork.GetRepository<Manufacture>().GetByPropertyAsync(m => m.Id == manufacture.Id);
					return (item, null);
				}
				return (null, $" ManufactureRepository.CreateAsync: ");
			}
			catch (Exception ex)
			{ 
				return (null, $" ManufactureRepository.CreateAsync: {ex.Message}");
			}
		}

		public async Task<(int Result, string? Error)> DeleteAsync(long id)
		{
			try
			{
				await _unitOfWork.GetRepository<Manufacture>().DeleteAsync(id);
				var result = await _unitOfWork.SaveAsync();	
				return (result, null);
			} catch (Exception ex)
			{
				return (0, $" ManufactureRepository.DeleteAsync: {ex.Message}");
			}
		}

		public async Task<(IEnumerable<Manufacture> Manufactures, string? Error)> GetAllAsync()
		{
			try
			{
				var manufactures = await _unitOfWork.GetRepository<Manufacture>().GetAllByPropertyAsync();
				return (manufactures, null);
			}
			catch (Exception ex)
			{
				return (Enumerable.Empty<Manufacture>(), $" ManufactureRepository.GetAllAsync: {ex.Message}");
			}
		}

		public async Task<(Manufacture? Manufacture, string? Error)> GetByIdAsync(long id)
		{
			try
			{
				var manufacture =  await _unitOfWork.GetRepository<Manufacture>().GetByPropertyAsync(m => m.Id == id);
				return (manufacture, null);
			} catch (Exception ex)
			{
				return (null, $" ManufactureRepository.GetByIdAsync: {ex.Message}");
			}
		}

		public async Task<(Manufacture? Manufacture, string? Error)> UpdateAsync(Manufacture manufacture)
		{
			try
			{
				await _unitOfWork.GetRepository<Manufacture>().UpdateAsync(manufacture);
				var result = await _unitOfWork.SaveAsync();
				if (result > 0)
				{
					var item = await _unitOfWork.GetRepository<Manufacture>().GetByPropertyAsync(m => m.Id == manufacture.Id);
					return (item, null);
				}
				return (null, $" ManufactureRepository.UpdateAsync: ");
			}
			catch (Exception ex)
			{
				return (null, $" ManufactureRepository.UpdateAsync: {ex.Message}");
			}
		}
	}
}
