using OEMEV.UserService.Application.Interfaces;

namespace OEMEV.UserService.Application.Services
{
	public class ServiceProviders : IServiceProviders
	{
		public IUserService UserService { get; }

		public IServiceCenterService ServiceCenterService { get; }

		public IManufactureService ManufactureService { get; }

		public IRoleService RoleService { get; }

		public ServiceProviders(
			IUserService userService,
			IServiceCenterService serviceCenterService,
			IManufactureService manufactureService,
			IRoleService roleService)
		{
			UserService = userService;
			ServiceCenterService = serviceCenterService;
			ManufactureService = manufactureService;
			RoleService = roleService;
		}
	}
}