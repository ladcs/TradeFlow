using RabbitMQ.Client;
using TradeFlow.RabbitMq.setup;
using TradeFlow.Orders.Dto;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BaseUrl>();

builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    var baseUrl = new BaseUrl(builder.Configuration);
    options.UseNpgsql(baseUrl.GetConnectionString());
});

builder.Services.AddSingleton<IConnection>(sp =>
{
    var config = builder.Configuration;
    var factory = new ConnectionFactory
    {
        HostName = config["RABBITMQ_HOST"] ?? "localhost",
        Port        = config.GetValue<int>("RABBITMQ_PORT", 5672),
        UserName    = config["RABBITMQ_USER"] ?? "guest",
        Password    = config["RABBITMQ_PASS"] ?? "guest",
        VirtualHost = config["RABBITMQ_VHOST"] ?? "/"
    };
    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});

builder.Services.AddSingleton<IChannel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();

    RabbitMqSetup.ConfigureAsync(channel, new QueueConfig(
        Exchange: "orders",
        DurableExchange: true,
        AutoDeleteExchange: false,
        Queue: "orders.processing",
        Exclusive: false,
        DurableQueue: true,
        AutoDeleteQueue: false
    )).GetAwaiter().GetResult();

    RabbitMqSetup.ConfigureAsync(channel, new QueueConfig(
        Exchange: "orders",
        DurableExchange: true,
        AutoDeleteExchange: false,
        Queue: "orders.notifications",
        Exclusive: false,
        DurableQueue: true,
        AutoDeleteQueue: false
    )).GetAwaiter().GetResult();

    return channel;
});

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

app.Services.GetRequiredService<IChannel>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.Run();