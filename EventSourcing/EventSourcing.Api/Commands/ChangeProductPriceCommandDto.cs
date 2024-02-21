using MediatR;

namespace EventSourcing.Api.Commands
{
    public class ChangeProductPriceCommandDto : IRequest
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
    }
}
