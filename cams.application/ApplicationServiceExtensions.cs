using cams.application.config;
using cams.application.repositories;
using cams.application.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cams.application;

public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registers application-level services and configuration settings with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration instance.</param>
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IBidderRepository, BidderRepository>();
        services.Configure<VehicleBidSettings>(configuration.GetSection("VehicleBidSettings"));
        services.Configure<AuctionSettings>(configuration.GetSection("AuctionSettings"));
        services.AddCamsServices();
    }

    /// <summary>
    /// Registers core CAMS services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    private static void AddCamsServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IBidderService, BidderService>();
    }
}