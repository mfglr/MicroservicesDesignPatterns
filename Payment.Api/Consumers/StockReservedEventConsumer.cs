using MassTransit;
using SharedLibrary;

namespace Payment.Api.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishser;

        public StockReservedEventConsumer(IPublishEndpoint publishser)
        {
            _publishser = publishser;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            decimal fakeBalance = 3000;

            if(fakeBalance < context.Message.Payment.TotalPrice)
            {
                await _publishser.Publish(new PaymentFailedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    Message = "",
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems
                });
                return;
            }

            await _publishser.Publish(new PaymentCompletedEvent()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
            });




        }
    }
}
