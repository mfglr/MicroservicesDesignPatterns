using Microsoft.EntityFrameworkCore;

namespace Order.Api.Entities
{
    [Owned]
    public class Address
    {
        public string Line { get; set; }

    }
}
