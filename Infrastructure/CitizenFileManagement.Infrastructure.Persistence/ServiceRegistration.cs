using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.Persistence.Concrete;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;
using CitizenFileManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CitizenFileManagement.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<CitizenFileDB>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.Scan(scan => scan
             .FromAssembliesOf(typeof(IRepository<>))
             .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
             .AsImplementedInterfaces()
             .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(Repository<>))
            .AddClasses(classes => classes.AssignableTo(typeof(Repository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IClaimManager, ClaimManager>();
        services.AddScoped<IUserManager, UserManager>();

    }
}
