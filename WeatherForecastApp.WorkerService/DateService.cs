using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherForecastApp.WorkerService.Dtos;


//namespace WeatherForecastApp.WorkerService;

public class DateService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
        
    public DateService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("WeatherforThatDAY", false, false, false, null);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += async (model,content) =>
        {
            var body = content.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var Climanfo = JsonConvert.DeserializeObject<WeatherformationDataTransferObject>(json);
            var ClimaResult = await ProcessDate(watherIn, cancellationToken);
            var message = $"EL clime para el dia {WeatherformationDataTransferObject.Date} Estado del request: {weatherResult.WeatherTransaction.Status}";
            Console.WriteLine(message);
            NOtiCLima(ClimaResult.WeatherTransaction);
        };
        _channel.BasicConsume("payment-queue", true, _consumer);
        return Task.CompletedTask;
    }


    private void NOtiCLima(WeatherTransaction WeatherTransation)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("WeatherforThatDAY", false, false, false, null);
                var json = JsonConvert.SerializeObject(WeatherTransation);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(string.Empty, "WeatherforThatDAY", null, body);
            }
        }
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Busqueda de Cilma en Fecha  ejecutandose {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
private void WeathrResults(WeatherTransation WeatherTransation)
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672
    };

    using (var connection = factory.CreateConnection())
    {
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare("WeatherforThatDAY", false, false, false, null);
            var json = JsonConvert.SerializeObject(WeatherTransation);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(string.Empty, "WeatherforThatDAY", null, body);
        }
    }
}
private async Task<WeatherInformationDataTransferObject> ProsseseDate(WeatherInformationDataTransferObject weatherInfo,
    CancellationToken token)
{
    var errors = new List<string>();
    if (weatherInfo.PaymentMethod.CardNumber == Guid.Empty)
    {
        errors.Add("La fecha no existe.");
    }

    await Task.Delay(1000, token);



    if (watherInfo.PaymentMethod.Month <= 0 || watherInfo.WeatherMethod.Year < 0)
    {
        errors.Add("Error.");
    }
    DayRequest.Date = dateW
            if (!int.TryParse(dateString, out dateW))
    {
        Console.WriteLine("Invalid date format");
        return;
    }


    var factory = new ConnectionFactory()
    {
        HostName = "localhost"
    };
    using (var connection = factory.CreateConnection())
    {
        using (var channel = connection.CreateModel())
        {
            // Create exchange
            channel.ExchangeDeclare("WeatherforThatDAY", ExchangeType.Fanout);

            // Create queue for the console app
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "WeatherforThatDAY", "");

            // Set up consumer for the console app
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var forecast = JsonConvert.DeserializeObject<Forecast>(message);
                Console.WriteLine($"Weather forecast for {forecast.Date}: {forecast.Forecast}");
            };
            channel.BasicConsume(queueName, true, consumer);

            // Send message to the worker service
            var message = JsonConvert.SerializeObject(new DateMessage { Date = dateW });
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("WeatherforThatDAY", "", null, body);

            // Wait for result
            Thread.Sleep(60000);
        }


        weatherInfo.WeatherTransation.Status = errors.Any() ? Status.Errored : Status.Done;
        weatherInfo.WeatherTransation.Errors = errors;
        return weatherInfo;
    }
}
        
           
    public class DateMessage
    {
        public int Date { get; set; }
    }

    public class Forecast
    {
        public int Date { get; set; }
        public string Summary { get; set; }
    }
}