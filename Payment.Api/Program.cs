using MassTransit;
using Payment.Api.Consumers;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockReservedRequestEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");
        cfg.ReceiveEndpoint(QueueNames.Payment_StockReservedRequestEventQueueName, e =>
        {
            e.ConfigureConsumer<StockReservedRequestEventConsumer>(context);
        });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
