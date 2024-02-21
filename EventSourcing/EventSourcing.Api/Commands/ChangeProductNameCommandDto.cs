using MediatR;

namespace EventSourcing.Api.Commands
{
    public class ChangeProductNameCommandDto : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
