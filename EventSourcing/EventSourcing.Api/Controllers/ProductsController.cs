using EventSourcing.Api.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommandDto request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductName(ChangeProductNameCommandDto request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductPrice(ChangeProductPriceCommandDto request)
        {
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _mediator.Send(new DeleteProductCommandDto() { Id = id});
            return NoContent();
        }



    }
}
