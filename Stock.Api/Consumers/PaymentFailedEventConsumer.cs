using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;

namespace Stock.Api.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {

        private readonly AppDbContext _appDbContext;

        public PaymentFailedEventConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var ids = context.Message.OrderItems.Select(x => x.ProductId);
            var stocks = await _appDbContext.Stocks.Where(x => ids.Contains(x.ProductId)).ToListAsync();

            foreach (var stock in stocks)
            {
                foreach (var item in context.Message.OrderItems) {
                    if(item.ProductId == stock.ProductId)
                        stock.Count += item.Count;
                }
            }
            await _appDbContext.SaveChangesAsync();

        }
    }
}
