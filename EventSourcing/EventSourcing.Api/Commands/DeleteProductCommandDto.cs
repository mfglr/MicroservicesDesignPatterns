using MediatR;

namespace EventSourcing.Api.Commands
{
    public class DeleteProductCommandDto : IRequest
    {
        public Guid Id { get; set; }
    }
}
