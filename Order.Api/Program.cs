using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>((sp, opt) => {
    var configuration = sp.GetRequiredService<IConfiguration>();
    opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
