using MassTransit;
using Order.Api.Entities;
using SharedLibrary.Abstracts;

namespace Order.Api.Consumer
{
    public class OrderCreationRequestCompletedEventConsumer : IConsumer<IOrderCreationRequestCompletedEvent>
    {

        private readonly AppDbContext _context;

        public OrderCreationRequestCompletedEventConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<IOrderCreationRequestCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            order.State = OrderState.Success;
            await _context.SaveChangesAsync();
        }
    }
}
