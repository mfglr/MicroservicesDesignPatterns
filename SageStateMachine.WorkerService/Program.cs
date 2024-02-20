using MassTransit;
using Microsoft.EntityFrameworkCore;
using SageStateMachine.WorkerService;
using SageStateMachine.WorkerService.Models;
using SharedLibrary;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddMassTransit(cfg =>
        {
            cfg
                .AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(opt =>
                {
                    opt.AddDbContext<DbContext,OrderStateDbContext>((sp,opt) =>
                    {
                        var configuration = sp.GetRequiredService<IConfiguration>();
                        opt.UseSqlServer(
                            configuration.GetConnectionString("SqlServer"),
                            m => m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
                        );
                    });
                });

            cfg.UsingRabbitMq((context,cfg) =>
            {
                cfg.Host("localhost");
                cfg.ReceiveEndpoint(QueueNames.OrderSaga,e =>
                {
                    e.ConfigureSaga<OrderState>(context);
                });
            });

        });

    })
    .Build();

host.Run();
