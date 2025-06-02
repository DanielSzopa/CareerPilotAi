using CareerPilotAi.Infrastructure.Identity;
using CareerPilotAi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPilotAi.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddIdentityExtensions()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            return services;
        }
    }
} 