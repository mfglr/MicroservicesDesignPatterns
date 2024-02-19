using MassTransit;
using Order.Api.Entities;
using SharedLibrary;

namespace Order.Api.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {

        private readonly AppDbContext _context;

        public StockNotReservedEventConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            order.State = OrderState.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
