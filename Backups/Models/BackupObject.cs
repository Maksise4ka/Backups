using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models;

public class BackupObject
{
    public BackupObject(string relativePath, IRepository repository)
    {
        if (!repository.ObjectExists(relativePath))
            throw new BackupObjectException(relativePath);

        RelativePath = relativePath;
        Id = Guid.NewGuid();
        Repository = repository;
    }

    public Guid Id { get; }
    public string RelativePath { get; }
    public IRepository Repository { get; }
}