using ReserveiAPI.Repositories.Interfaces;
using ReserveiAPI.Repositories.Entities;
using ReserveiAPI.Services.Entities;
using ReserveiAPI.Services.interfaces;
namespace ReserveiAPI.Services.Server
{
	public static class DependeciesInjection
	{
		public static void AddUserDepencies(this IServiceCollection services)
		{
			//dependencia : usuario
			services.AddScoped<IUserRepository, UserRepository>();
				services.AddScoped<IUserService, UserService>();
		}
	}
}
