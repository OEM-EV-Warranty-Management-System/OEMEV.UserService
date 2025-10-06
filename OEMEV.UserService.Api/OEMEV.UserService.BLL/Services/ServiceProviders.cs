using OEMEV.UserService.BLL.Interfaces;

namespace OEMEV.UserService.BLL.Services
{
	public class ServiceProviders : IServiceProviders
	{
		public IUserService UserService { get; }

		public ServiceProviders(IUserService userService)
		{
			UserService = userService;
		}
	}
}
