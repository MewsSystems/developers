namespace Exchange.Infrastructure.DateTimeProviders;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}