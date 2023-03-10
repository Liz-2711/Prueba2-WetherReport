namespace WeatherForecastApp.WorkerService;

public class WeatherTransaction
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