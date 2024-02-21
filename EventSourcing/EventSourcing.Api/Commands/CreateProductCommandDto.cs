using MediatR;

namespace EventSourcing.Api.Commands
{
    public class CreateProductCommandDto : IRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
