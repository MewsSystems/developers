namespace Common.Exceptions;

public class InvalidContentException : Exception
{
    #region Constructors

    public InvalidContentException(string? message) : base(message) { }

    #endregion
}