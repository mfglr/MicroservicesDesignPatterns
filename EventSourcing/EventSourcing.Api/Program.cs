using EventSourcing.Api;
using EventSourcing.Api.BackgroundServices;
using EventSourcing.Api.Models;
using EventSourcing.Api.Streams;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
await builder.Services.AddEventStoreConnectionAsync();
builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var streamName = configuration.GetSection("ProductStream")["StreamName"]!;
    return new ProductEventContainer(streamName, sp.GetRequiredService<IEventStoreConnection>());
});



builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddDbContext<AppDbContext>((sp, builder) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    builder.UseSqlServer(configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddHostedService<ProductReadModelSubscription>();
//builder.Services.AddHostedService<ReplayProductsSubscription>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
