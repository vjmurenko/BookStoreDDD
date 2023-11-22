using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEFRepositories(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        },
        ServiceLifetime.Transient);

        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IBookRepository, BookRepository>();

        return services;
    }
}