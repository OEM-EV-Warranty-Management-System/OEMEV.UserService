using OEMEV.UserService.Domain.Models;

namespace OEMEV.UserService.Infrastructure.Interfaces
{
	public interface IManufactureRepository
	{
		Task<(IEnumerable<Manufacture> Manufactures, string? Error)> GetAllAsync();
		Task<(Manufacture? Manufacture, string? Error)> GetByIdAsync(long id);
		Task<(Manufacture? Manufacture, string? Error)> CreateAsync(Manufacture manufacture);
		Task<(Manufacture? Manufacture, string? Error)> UpdateAsync(Manufacture manufacture);
		Task<(int Result, string? Error)> DeleteAsync(long id);
	}
}
