using cams.application.config;
using cams.application.repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cams.application;

public static class ApplicationServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.Configure<VehicleBidSettings>(configuration.GetSection("VehicleBidSettings"));

    }
}
