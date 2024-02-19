using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;

namespace Stock.Api.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ISendEndpointProvider _sender;
        private readonly IPublishEndpoint _publisher;

        public OrderCreatedEventConsumer(AppDbContext appDbContext, ISendEndpointProvider sender, IPublishEndpoint publisher)
        {
            _appDbContext = appDbContext;
            _sender = sender;
            _publisher = publisher;
        }


        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {

            var ids = context.Message.OrderItems.Select(x => x.ProductId).ToList();

            var stocks = await _appDbContext.Stocks.Where(x => ids.Contains(x.ProductId)).ToListAsync();

            if(stocks.Count == 0)
            {
                await _publisher.Publish(new StockNotReservedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    Message = "",
                    OrderId = context.Message.OrderId
                });
                return;
            }

            foreach (var item in context.Message.OrderItems) { 
                if (stocks.Any(x => item.Count > x.Count))
                {
                    await _publisher.Publish(new StockNotReservedEvent()
                    {
                        BuyerId = context.Message.BuyerId,
                        Message = "",
                        OrderId = context.Message.OrderId
                    });
                    return;
                }
            }

            foreach(var stock in stocks)
            {
                foreach(var item in context.Message.OrderItems)
                {
                    if(stock.ProductId == item.ProductId) 
                        stock.Count -= item.Count; 
                }
            }

            await _appDbContext.SaveChangesAsync();

            var endPoint = await _sender.GetSendEndpoint(
                new Uri($"queue:{QueueNames.StockReservedEventQueueName}")
            );

            await endPoint.Send(new StockReservedEvent()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                OrderItems = context.Message.OrderItems,
                Payment = context.Message.Payment
            });


        }
    }
}
