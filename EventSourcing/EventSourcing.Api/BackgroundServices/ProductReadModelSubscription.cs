using EventSourcing.Api.Models;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace EventSourcing.Api.BackgroundServices
{
    public class ProductReadModelSubscription : BackgroundService
    {

        private readonly IEventStoreConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public ProductReadModelSubscription(IEventStoreConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            await _connection.ConnectToPersistentSubscriptionAsync(
                configuration.GetSection("ProductStream")["StreamName"],
                configuration.GetSection("ProductStream")["GroupName"],
                Calback,
                autoAck : false
            );
        }


        private async Task Calback(EventStorePersistentSubscriptionBase arg1, ResolvedEvent arg2)
        {

            var assemblyName = Assembly.GetAssembly(typeof(IEvent)).GetName().Name;
            var type = Type.GetType($"{Encoding.UTF8.GetString(arg2.Event.Metadata)}, {assemblyName}");
            var eventData = Encoding.UTF8.GetString(arg2.Event.Data);

            var @event = JsonConvert.DeserializeObject(eventData, type);

            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Product product = null;

            switch (@event)
            {
                case ProductCreatedEvent productCreatedEvent:

                    product = new Product()
                    {
                        Name = productCreatedEvent.Name,
                        Id = productCreatedEvent.Id,
                        Price = productCreatedEvent.Price,
                        Stock = productCreatedEvent.Stock,
                        UserId = productCreatedEvent.UserId
                    };
                    context.Products.Add(product);
                    break;

                case ProductNameChangedEvent productNameChangedEvent:

                    product = context.Products.Find(productNameChangedEvent.Id);
                    if (product != null)
                    {
                        product.Name = productNameChangedEvent.Name;
                    }
                    break;

                case ProductPriceChangedEvent productPriceChangedEvent:
                    product = context.Products.Find(productPriceChangedEvent.Id);
                    if (product != null)
                    {
                        product.Price = productPriceChangedEvent.Price;
                    }
                    break;

                case ProductDeletedEvent productDeletedEvent:
                    product = context.Products.Find(productDeletedEvent.Id);
                    if (product != null)
                    {
                        context.Products.Remove(product);
                    }
                    break;
            }

            await context.SaveChangesAsync();

            arg1.Acknowledge(arg2.Event.EventId);
        }
    }
}
