using MassTransit;
using Order.Api.Entities;
using SharedLibrary.Abstracts;

namespace Order.Api.Consumer
{
    public class OrderCreationRequestFailedEventConsumer : IConsumer<IOrderCreationRequestFailedEvent>
    {
        private readonly AppDbContext _context;

        public OrderCreationRequestFailedEventConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<IOrderCreationRequestFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            order.State = OrderState.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
