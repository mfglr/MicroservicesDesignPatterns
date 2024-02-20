using MassTransit;
using SharedLibrary;
using SharedLibrary.Abstracts;
using SharedLibrary.Events;

namespace SageStateMachine.WorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {

        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public State OrderCreated { get; private set; }

        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public State StockReserved { get; set; }

        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
        public State StockNotReserved { get; set; }


        public Event<IPayment_StockReservedReuqestEvent> Payment_StockReservedReuqestEvent { get; set; }

        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public State PaymentCompleted { get; set; }


        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }
        public State PaymentFailed { get; set; }
        
        public OrderStateMachine() {
            InstanceState(x => x.CurrentState);
            Event(
                () => OrderCreatedRequestEvent,
                cfg => cfg
                    .CorrelateById<int>(
                        orderState => orderState.OrderId,
                        orderCreatedRequestEventContext => orderCreatedRequestEventContext.Message.OrderId
                    )
                    .SelectId(context => Guid.NewGuid())
            );

            Event(
                () => StockReservedEvent,
                cfg => cfg.CorrelateById(consumeContext => consumeContext.Message.CorrelationId)
            );

            Event(
                () => StockNotReservedEvent,
                cfg => cfg.CorrelateById(consumeContext => consumeContext.Message.CorrelationId)
            );

            Event(
                () => PaymentCompletedEvent,
                cfg => cfg.CorrelateById(consumeContext => consumeContext.Message.CorrelationId)
            );

            Event(
                () => PaymentFailedEvent,
                cfg => cfg.CorrelateById(consumeContext => consumeContext.Message.CorrelationId)
            );

            Initially(
                When(OrderCreatedRequestEvent)
                .Then(context =>
                {
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.BuyerId = context.Data.BuyerId;
                    context.Instance.CreatedDate = DateTime.Now;

                    context.Instance.CardName = context.Data.Payment.CardName;
                    context.Instance.CardNumber = context.Data.Payment.CardNumber;
                    context.Instance.CVV = context.Data.Payment.CVV;
                    context.Instance.Expiration = context.Data.Payment.Expiration;
                    context.Instance.TotalPrice = context.Data.Payment.TotalPrice;
                })
                .TransitionTo(OrderCreated)
                .Publish(
                    context => new OrderCreatedEvent() {
                        CorrelationId = context.Instance.CorrelationId,
                        OrderItems = context.Data.OrderItems,
                    }
                )
            );

            During(
                OrderCreated,
                When(StockReservedEvent)
                .Send(
                    new Uri($"queue:{QueueNames.Payment_StockReservedRequestEventQueueName}"),
                    context => new Payment_StockReservedReuqestEvent()
                    {
                        CorrelationId = context.Message.CorrelationId,
                        OrderItems = context.Message.OrderItems,
                        Payment =  new PaymentMessage()
                        {
                            CardName = context.Instance.CardName,
                            CardNumber = context.Instance.CardNumber,
                            CVV = context.Instance.CVV,
                            Expiration = context.Instance.Expiration,
                            TotalPrice = context.Instance.TotalPrice
                        }
                    }
                )
                .TransitionTo(StockReserved),

                When(StockNotReservedEvent)
                .Publish(context => new OrderCreationRequestFailedEvent()
                {
                    Message = context.Data.Message,
                    OrderId = context.Instance.OrderId
                })
                .TransitionTo(StockNotReserved)
            );


            During(
                StockReserved,
                
                When(PaymentCompletedEvent)
                .Publish(context => new OrderCreationRequestCompletedEvent()
                {
                    OrderId = context.Instance.OrderId
                })
                .TransitionTo(PaymentCompleted)
                .Finalize(),

                When(PaymentFailedEvent)
                .Publish(context => new OrderCreationRequestFailedEvent()
                {
                    Message = context.Data.Message,
                    OrderId = context.Instance.OrderId
                })
                .Send(
                    new Uri($"queue:{QueueNames.StockCompansableTransactionMessageQueue}"),
                    context => new StockCompansableTransactionMessage()
                    {
                        OrderItems = context.Data.OrderItems
                    }
                )
                .TransitionTo(PaymentFailed)
            );

            SetCompletedWhenFinalized();

        }

    }
}
