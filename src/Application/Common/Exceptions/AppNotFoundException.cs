namespace CleanArchitecture.Application.Common.Exceptions;

public class AppNotFoundException : Exception
{
    public AppNotFoundException()
        : base()
    {
    }

    public AppNotFoundException(string message)
        : base(message)
    {
    }

    public AppNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public AppNotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
