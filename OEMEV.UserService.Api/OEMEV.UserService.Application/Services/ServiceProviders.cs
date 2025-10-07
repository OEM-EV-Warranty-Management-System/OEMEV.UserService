using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Application.Services
{
	public class ServiceProviders : IServiceProviders
	{
		public IUserService UserService { get; }

		public IServiceCenterService ServiceCenterService { get;  }

		public ServiceProviders(IUserService userService, IServiceCenterService serviceCenterService)
		{
			UserService = userService;
			ServiceCenterService = serviceCenterService;
		}
	}
}
