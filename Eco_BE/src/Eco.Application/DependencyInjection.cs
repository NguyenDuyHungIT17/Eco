using Eco.Application.Common.Interfaces.Identity;
using Eco.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Eco.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
