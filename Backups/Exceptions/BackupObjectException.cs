namespace Backups.Exceptions;

public class BackupObjectException : BackupsException
{
    public BackupObjectException(string path)
        : base($@"File or Directory {path} is not exist")
    { }
}