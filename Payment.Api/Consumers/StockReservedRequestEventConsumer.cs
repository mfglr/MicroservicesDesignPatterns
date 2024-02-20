using MassTransit;
using SharedLibrary.Abstracts;
using SharedLibrary.Events;

namespace Payment.Api.Consumers
{
    public class StockReservedRequestEventConsumer : IConsumer<IPayment_StockReservedReuqestEvent>
    {

        private readonly IPublishEndpoint _publisher;

        public StockReservedRequestEventConsumer(IPublishEndpoint publisher)
        {
            _publisher = publisher;
        }

        public async Task Consume(ConsumeContext<IPayment_StockReservedReuqestEvent> context)
        {

            var fakeBalance = 3000;
            
            if(context.Message.Payment.TotalPrice > fakeBalance)
            {
                await _publisher.Publish(new PaymentFailedEvent()
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderItems = context.Message.OrderItems,
                    Message = "The balance is not enough!"
                });
                return;
            }

            await _publisher.Publish(new PaymentCompletedEvent()
            {
                CorrelationId = context.Message.CorrelationId,
            });

        }
    }
}
