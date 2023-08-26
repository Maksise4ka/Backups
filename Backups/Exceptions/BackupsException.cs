namespace Backups.Exceptions;

public class BackupsException : Exception
{
    public BackupsException()
    { }

    public BackupsException(string message)
        : base(message)
    { }
}