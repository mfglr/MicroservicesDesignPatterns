using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Stock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Models.Stock>> GetStocksAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

    }
}
