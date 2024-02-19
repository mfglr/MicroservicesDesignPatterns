using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Dtos;
using Order.Api.Entities;
using SharedLibrary;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly AppDbContext _context;
        private IPublishEndpoint _publisher;

        public OrdersController(AppDbContext context, IPublishEndpoint publisher)
        {
            _context = context;
            _publisher = publisher;
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

            var @event = new OrderCreatedEvent()
            {
                BuyerId = request.BuyerId,
                OrderId = order.Id,
                Payment = new PaymentMessage()
                {
                    CardName = request.Payment.CardName,
                    CartNumber = request.Payment.CartNumber,
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
           await _publisher.Publish(@event);

            return Ok();
        }
    }
}
