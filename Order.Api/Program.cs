using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Consumer;
using Order.Api.Entities;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>((sp, opt) => {
    var configuration = sp.GetRequiredService<IConfiguration>();
    opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockNotReservedEventConsumer>();
    x.AddConsumer<PaymentCompletedEventConsumer>();
    x.AddConsumer<PaymentFailedEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");
        
        cfg.ReceiveEndpoint(QueueNames.Order_StockNotReservedEventQueueName, e =>
        {
            e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.Order_PaymentCompletedEventQueueName, e =>
        {
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.Order_PaymentFailedEventQueueName, e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });

    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
