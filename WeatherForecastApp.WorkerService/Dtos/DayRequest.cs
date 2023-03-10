namespace WeatherForecastApp.WorkerService.Dtos;

public class PaymentTransaction
{
    public Guid Id { get; set; }

    public Status Status { get; set; }

    public List<string> Errors { get; set; }
}

public enum Status
{
    InProgress,
    Done,
    Errored
}