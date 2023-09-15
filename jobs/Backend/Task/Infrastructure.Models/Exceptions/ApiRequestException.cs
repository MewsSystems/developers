namespace Infrastructure.Models.Exceptions;

public class ApiRequestException : Exception
{
    public ApiRequestException() { }
    public ApiRequestException(string message) : base(message) { }
}
