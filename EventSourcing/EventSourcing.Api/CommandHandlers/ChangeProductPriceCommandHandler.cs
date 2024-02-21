using AutoMapper;
using EventSourcing.Api.Commands;
using EventSourcing.Api.Streams;
using EventSourcing.Shared.Events;
using MediatR;

namespace EventSourcing.Api.CommandHandlers
{
    public class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommandDto>
    {
        private readonly IMapper _mapper;
        private readonly ProductEventContainer _container;

        public ChangeProductPriceCommandHandler(IMapper mapper, ProductEventContainer container)
        {
            _mapper = mapper;
            _container = container;
        }

        public async Task Handle(ChangeProductPriceCommandDto request, CancellationToken cancellationToken)
        {
            _container.AddEvent(_mapper.Map<ProductPriceChangedEvent>(request));
            await _container.SaveAsync();
        }
    }
}
