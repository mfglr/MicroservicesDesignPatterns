using MassTransit;
using SharedLibrary.Abstracts;

namespace Stock.Api.Consumers
{
    public class StockCompansableQueueMessageConsumer : IConsumer<IStockCompansableTransactionMessage>
    {
        private readonly AppDbContext _context;

        public StockCompansableQueueMessageConsumer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<IStockCompansableTransactionMessage> context)
        {
            var ids = context.Message.OrderItems.Select(x => x.ProductId).ToList();
            var stocks = _context.Stocks.Where(x =>  ids.Contains(x.ProductId)).ToList();

            foreach(var stock in stocks)
            {
                foreach(var item in context.Message.OrderItems)
                {
                    if (item.ProductId == stock.ProductId)
                        stock.Count += item.Count;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
