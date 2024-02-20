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
    x.AddConsumer<OrderCreationRequestCompletedEventConsumer>();
    x.AddConsumer<OrderCreationRequestFailedEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");
        cfg.ReceiveEndpoint(QueueNames.Order_OrderCreattionRequestCompletedQueue, e =>
        {
            e.ConfigureConsumer<OrderCreationRequestCompletedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(QueueNames.Order_OrderCreattionRequestFailedQueue, e =>
        {
            e.ConfigureConsumer<OrderCreationRequestFailedEventConsumer>(context);
        });

    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
