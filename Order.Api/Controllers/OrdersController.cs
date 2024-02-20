using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Dtos;
using Order.Api.Entities;
using SharedLibrary;
using SharedLibrary.Abstracts;
using SharedLibrary.Events;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrdersController(AppDbContext context, IPublishEndpoint publisher, ISendEndpointProvider sendEndpointProvider)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto request)
        {
            
                var order = new Entities.Order()
            {
                BuyerId = request.BuyerId,
                Address = new Address
                {
                    Line = request.Address.Line,
                },
                CreatedDate = DateTime.Now,
                State = OrderState.Suspend,
                Items = request.OrderItems.Select(x => new OrderItem()
                {
                    Count = x.Count,
                    Price = x.Price,
                    ProductId = x.ProductId,
                }).ToList()
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            var @event = new OrderCreatedRequestEvent()
            {
                BuyerId = request.BuyerId,
                OrderId = order.Id,
                Payment = new PaymentMessage()
                {
                    CardName = request.Payment.CardName,
                    CardNumber = request.Payment.CardNumber,
                    CVV = request.Payment.CVV,
                    Expiration = request.Payment.Expiration,
                    TotalPrice = request.OrderItems.Sum(x => x.Price * x.Count),
                },
                OrderItems = request.OrderItems.Select(x => new OrderItemMessage()
                {
                    Count = x.Count,
                    ProductId = x.ProductId
                }).ToList()
            };
            var sender = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueNames.OrderSaga}"));
            await sender.Send<IOrderCreatedRequestEvent>(@event);

            return Ok();
        }
    }
}
