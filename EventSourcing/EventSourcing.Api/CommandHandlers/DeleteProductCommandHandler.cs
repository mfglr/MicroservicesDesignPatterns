using AutoMapper;
using EventSourcing.Api.Commands;
using EventSourcing.Api.Streams;
using EventSourcing.Shared.Events;
using MediatR;

namespace EventSourcing.Api.CommandHandlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandDto>
    {

        private readonly IMapper _mapper;
        private readonly ProductEventContainer _container;

        public DeleteProductCommandHandler(IMapper mapper, ProductEventContainer container)
        {
            _mapper = mapper;
            _container = container;
        }

        public async Task Handle(DeleteProductCommandDto request, CancellationToken cancellationToken)
        {
            _container.AddEvent(_mapper.Map<ProductDeletedEvent>(request));
            await _container.SaveAsync();
        }
    }
}
