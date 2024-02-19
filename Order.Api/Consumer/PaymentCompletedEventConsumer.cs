using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Entities;
using SharedLibrary;

namespace Order.Api.Consumer
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {

        private readonly AppDbContext _context;
        public PaymentCompletedEventConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            order.State = OrderState.Success;
            await _context.SaveChangesAsync();
        }
    }
}
