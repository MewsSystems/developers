namespace ExchangeRateUpdater.Domain.CoreModels;

public class ExceptionResponseObject
{
  public string Message { get; set; }
  public string CallStack { get; set; }

  public ExceptionResponseObject() { }
  
  public ExceptionResponseObject(Exception exception)
  {
    Exception baseException = exception.GetBaseException();
    Message = baseException.Message;
    CallStack = baseException.StackTrace ?? string.Empty;
  }
}