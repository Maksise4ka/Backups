using Backups.Interfaces;

namespace Backups.Models;

public class RestorePoint
{
    public RestorePoint(IStorage storage, IEnumerable<BackupObject> backupObjects, DateTime dateTime, Guid id)
    {
        DateTime = dateTime;
        Storage = storage;
        BackupObjects = backupObjects;
        Id = id;
    }

    public DateTime DateTime { get; }
    public Guid Id { get; }

    public IStorage Storage { get; }
    public IEnumerable<BackupObject> BackupObjects { get; }
}