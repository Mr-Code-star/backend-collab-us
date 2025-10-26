namespace backend_collab_us.Shared.Domain.Exeptions;

public class GeneralException : Exception
{
    public string Code { get; }

    public GeneralException(string message, string code)
        : base(message)
    {
        Code = code;
    }

    public GeneralException(string message, string code, Exception innerException)
        : base(message, innerException)
    {
        Code = code;
    }
}