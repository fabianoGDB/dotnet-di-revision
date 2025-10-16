using DependencyStore.Repositories.Contracts;
using DependencyStore.Repositories;
using DependencyStore.Services.Contracts;
using DependencyStore.Services;
using Microsoft.Data.SqlClient;

namespace DependencyStore
{
    public static class DependenciesExtension
    {
        public static void AddSqlConnection(this IServiceCollection services, string connectionString)
        {
            services.AddScoped(x => new SqlConnection(connectionString));
        }
        public static void AddRepositories(this IServiceCollection services)
        {

            services.AddTransient<ICostumerRepository, CostumerRepository>();
            services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
            services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
        }
    }
}
