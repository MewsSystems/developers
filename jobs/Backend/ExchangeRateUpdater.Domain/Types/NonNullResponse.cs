namespace ExchangeRateUpdater.Domain.Types;

public class NonNullResponse<T>
{
    public T Content { get; }
    public bool IsSuccess { get; }
    public List<string> Warnings { get; }
    public List<string> Errors { get; }

    private NonNullResponse(T content, bool isSuccess)
    {
        Content = content;
        IsSuccess = isSuccess;
        Errors = new List<string>();
        Warnings = new List<string>();
    }

    public static NonNullResponse<T> Success(T content)
    {
        return new NonNullResponse<T>(content,true);
    }
    public static NonNullResponse<T> Fail(T content,string error)
    {
        return new NonNullResponse<T>(content,false).AddErrorMessage(error);
    }

    public NonNullResponse<T> AddErrorMessage(string error)
    {
        Errors.Add(error);
        return this;
    }     
    
    public NonNullResponse<T> AddWarningMessage(string warning)
    {
        Warnings.Add(warning);
        return this;
    }    
    

}