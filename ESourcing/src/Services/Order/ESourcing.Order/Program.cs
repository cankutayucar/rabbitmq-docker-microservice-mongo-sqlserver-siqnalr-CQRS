using ESourcing.Order.Consumers;
using ESourcing.Order.Extensions;
using EventBusRabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

//builder.Services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(builder.Configuration.GetSection("constr").Value, b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)), ServiceLifetime.Singleton);
//typeof(OrderContext).Assembly.FullName
builder.Services.AddDbContext<OrderContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection"), op => op.MigrationsAssembly(typeof(OrderContext).Assembly.FullName));
}, ServiceLifetime.Transient);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Api", Version = "v1" });
});
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(opt =>
{
    var logger = opt.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBus:HostName"]
    };
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:UserName"]))
    {
        factory.UserName = builder.Configuration["EventBus:UserName"];
    }
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:Password"]))
    {
        factory.Password = builder.Configuration["EventBus:Password"];
    }
    var retryCount = 5;
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:RetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBus:RetryCount"].ToString());
    }
    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
});
builder.Services.AddSingleton<EventBusOrderCreateConsumer>();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Api v1");
    });
}


app.MigrateDatabase();

app.UseEventBusListener();

app.UseAuthorization();

app.MapControllers();

app.Run();
