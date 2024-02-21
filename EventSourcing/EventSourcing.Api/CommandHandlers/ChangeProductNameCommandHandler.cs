using AutoMapper;
using EventSourcing.Api.Commands;
using EventSourcing.Api.Streams;
using EventSourcing.Shared.Events;
using MediatR;

namespace EventSourcing.Api.CommandHandlers
{
    public class ChangeProductNameCommandHandler : IRequestHandler<ChangeProductNameCommandDto>
    {

        private readonly IMapper _mapper;
        private readonly ProductEventContainer _container;

        public ChangeProductNameCommandHandler(IMapper mapper, ProductEventContainer container)
        {
            _mapper = mapper;
            _container = container;
        }

        public async Task Handle(ChangeProductNameCommandDto request, CancellationToken cancellationToken)
        {
            _container.AddEvent(_mapper.Map<ProductNameChangedEvent>(request));
            await _container.SaveAsync();
        }
    }
}
