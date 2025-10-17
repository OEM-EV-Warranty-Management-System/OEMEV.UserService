namespace OEMEV.UserService.Application.Interfaces
{
	public interface IServiceProviders
	{
		IUserService UserService { get; }

		IServiceCenterService ServiceCenterService { get; }

		IManufactureService ManufactureService { get; }
	}
}
