using System;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;
using CitizenFileManagement.Infrastructure.External.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CitizenFileManagement.Infrastructure.External;

public static class ServiceRegistration
{
    public static void AddExternalRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMinIOService, MinioService>();
    }
}
