using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using Stock.Api;
using Stock.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("StockDb");
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();
    x.AddConsumer<StockCompansableQueueMessageConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");
        cfg.ReceiveEndpoint(QueueNames.Stock_OrderCreateEventQueueName, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(QueueNames.StockCompansableTransactionMessageQueue, e =>
        {
            e.ConfigureConsumer<StockCompansableQueueMessageConsumer>(context);
        });
    });
});


var app = builder.Build();



using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Stocks.Add(new Stock.Api.Models.Stock(){ Id = 1, Count = 105,ProductId = 1});
    context.Stocks.Add(new Stock.Api.Models.Stock() { Id = 2, Count = 55, ProductId = 2 });
    context.SaveChanges();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
