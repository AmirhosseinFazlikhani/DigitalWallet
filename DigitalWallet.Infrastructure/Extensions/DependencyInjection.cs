using DigitalWallet.Application.Services;
using DigitalWallet.Domain.Repositories;
using DigitalWallet.Infrastructure.Internals;
using DigitalWallet.Infrastructure.Repositories;
using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("EventStore");
            var settings = EventStoreClientSettings.Create(connectionString);

            services.AddSingleton((provider) => new EventStoreClient(settings));

            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IEventSource, EventSource>();
        }

        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<GetService>();
            services.AddTransient<CreateService>();
            services.AddTransient<DepositService>();
            services.AddTransient<WithdrawService>();
        }
    }
}
