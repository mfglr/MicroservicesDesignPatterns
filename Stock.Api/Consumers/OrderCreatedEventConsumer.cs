using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Abstracts;
using SharedLibrary.Events;

namespace Stock.Api.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPublishEndpoint _publisher;

        public OrderCreatedEventConsumer(AppDbContext appDbContext, IPublishEndpoint publisher)
        {
            _appDbContext = appDbContext;
            _publisher = publisher;
        }


        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {

            var ids = context.Message.OrderItems.Select(x => x.ProductId).ToList();

            var stocks = await _appDbContext.Stocks.Where(x => ids.Contains(x.ProductId)).ToListAsync();

            if (stocks.Count == 0)
            {
                await _publisher.Publish(new StockNotReservedEvent()
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = "Product not found"
                });
                return;
            }

            foreach (var item in context.Message.OrderItems)
            {
                if (stocks.Any(x => item.Count > x.Count))
                {
                    await _publisher.Publish(new StockNotReservedEvent()
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Message = "Not enough stock",
                    });
                    return;
                }
            }

            foreach (var stock in stocks)
            {
                foreach (var item in context.Message.OrderItems)
                {
                    if (stock.ProductId == item.ProductId)
                        stock.Count -= item.Count;
                }
            }

            await _appDbContext.SaveChangesAsync();

            await _publisher.Publish(new StockReservedEvent()
            {
                CorrelationId = context.Message.CorrelationId,
                OrderItems = context.Message.OrderItems,
            });


        }
    }
}
