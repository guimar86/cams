using cams.application.config;
using cams.application.repositories;
using cams.application.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cams.application;

public static class ApplicationServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IBidderRepository, BidderRepository>();
        services.Configure<VehicleBidSettings>(configuration.GetSection("VehicleBidSettings"));
        services.AddCamsServices();
    }
    
    public static IServiceCollection AddCamsServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IBidderService, BidderService>();
        return services;
    }
}
