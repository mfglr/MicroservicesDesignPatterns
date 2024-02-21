using EventStore.ClientAPI;

namespace EventSourcing.Api
{
    public static class Extentions
    {

        public static async Task<IServiceCollection> AddEventStoreConnectionAsync(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("EventStore")!;
            var connection = EventStoreConnection.Create(connectionString);
            await connection.ConnectAsync();
            
            return services.AddSingleton(connection);
        }

    }
}
