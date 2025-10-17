using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Application.Services
{
	public class ServiceProviders : IServiceProviders
	{
		public IUserService UserService { get; }

		public IServiceCenterService ServiceCenterService { get; }

		public IManufactureService ManufactureService { get; set; }

		public ServiceProviders(IUserService userService, IServiceCenterService serviceCenterService, IManufactureService manufactureService)
		{
			UserService = userService;
			ServiceCenterService = serviceCenterService;
			ManufactureService = manufactureService;
		}
	}
}
