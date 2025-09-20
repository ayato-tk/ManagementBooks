using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using GestaoLivros.Infra.Data.Context;
using GestaoLivros.Infra.Data.Reports;
using GestaoLivros.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoLivros.Infra.Data;

public static class ServiceCollectionExtensions
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddServices(services);
        AddDbContext(services, configuration);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IReportService, ReportService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}