using AutoMapper;
using EventSourcing.Api.Commands;
using EventSourcing.Api.Streams;
using EventSourcing.Shared.Events;
using MediatR;

namespace EventSourcing.Api.CommandHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandDto>
    {

        private readonly ProductEventContainer _eventContainer;
        private readonly IMapper _mapper;


        public CreateProductCommandHandler(ProductEventContainer eventContainer, IMapper mapper)
        {
            _eventContainer = eventContainer;
            _mapper = mapper;
        }

        public async Task Handle(CreateProductCommandDto request, CancellationToken cancellationToken)
        {
            var @event = _mapper.Map<ProductCreatedEvent>(request);
            @event.Id = Guid.NewGuid();
            _eventContainer.AddEvent(@event);
            await _eventContainer.SaveAsync();
        }
    }
}
