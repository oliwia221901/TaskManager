using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagerAPI.Application
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

