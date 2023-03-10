using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TicketShop.Gateway.BackgroundServices;

public class DayRequest : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public DayRequest()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("WeatherForThatDAY", false, false, false, null);
        _consumer = new EventingBasicConsumer(_channel);
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += async (model,content) =>
        {
            var body = content.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var paymentInformation = JsonConvert.DeserializeObject<DayRequest>(json);
            
        };
        _channel.BasicConsume("WeatherForThatDAY", true, _consumer);
        return Task.CompletedTask;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Fecha request {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}