using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands.OrderCreate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.Tracing;
using System.Text;

namespace ESourcing.Order.Consumers
{
    public class EventBusOrderCreateConsumer
    {
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public EventBusOrderCreateConsumer(IRabbitMQPersistentConnection rabbitMQPersistentConnection, IMapper mapper, IMediator mediator)
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            _mapper = mapper;
            _mediator = mediator;
        }

        public void Consume()
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }
            var channel = _rabbitMQPersistentConnection.CreateModel();
            channel.QueueDeclare(EventBusConstants.OrderCreateQueueName, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (object? sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message);
                if (e.RoutingKey == EventBusConstants.OrderCreateQueueName)
                {
                    var command = _mapper.Map<OrderCreateCommand>(@event);
                    command.CreatedAt = DateTime.UtcNow;
                    command.TotalPrice = @event.Quantity * @event.Price;
                    command.UnitPrice = @event.Price;
                    var result = await _mediator.Send(command);
                }
            };
            channel.BasicConsume(queue: EventBusConstants.OrderCreateQueueName, autoAck: true, consumer: consumer);
        }

        public void Disconnect()
        {
            _rabbitMQPersistentConnection.Dispose();
        }
    }
}
