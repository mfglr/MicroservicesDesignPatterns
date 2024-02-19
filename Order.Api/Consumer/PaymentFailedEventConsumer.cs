using MassTransit;
using Order.Api.Entities;
using SharedLibrary;

namespace Order.Api.Consumer
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {

        private readonly AppDbContext _context;

        public PaymentFailedEventConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            order.State = OrderState.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
