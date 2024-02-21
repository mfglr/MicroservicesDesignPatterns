using AutoMapper;
using EventSourcing.Api.Commands;
using EventSourcing.Shared.Events;

namespace EventSourcing.Api.Mappers
{
    public class ProductStreamMapper : Profile
    {
        public ProductStreamMapper()
        {
            CreateMap<ChangeProductNameCommandDto, ProductNameChangedEvent>();
            CreateMap<ChangeProductPriceCommandDto, ProductPriceChangedEvent>();
            CreateMap<CreateProductCommandDto, ProductCreatedEvent>();
            CreateMap<DeleteProductCommandDto, ProductDeletedEvent>();
        }

    }
}
