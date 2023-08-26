using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    private readonly List<BackupObject> _backupObjects;
    private readonly IBackup _backup;

    public BackupTask(string name, IRepository repository, IBackup backup, IAlgorithm algorithm)
    {
        Repository = repository;
        Name = name;
        Algorithm = algorithm;
        _backupObjects = new List<BackupObject>();
        _backup = backup;

        Repository.CreateDirectoryFromRoot(name);
    }

    public string Name { get; }
    public IReadOnlyList<BackupObject> BackupObjects => _backupObjects;
    public IAlgorithm Algorithm { get; }
    public IRepository Repository { get; }

    public string TaskPath => Name;

    public void AddBackupObject(BackupObject backupObject)
    {
        if (_backupObjects.Contains(backupObject))
            throw new BackupsException($@"Task {Name} is already tracking {backupObject.RelativePath}");

        if (!Repository.ObjectExists(backupObject.RelativePath))
            throw new BackupObjectException(backupObject.RelativePath);

        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        if (!_backupObjects.Contains(backupObject))
            throw new BackupsException($@"Task {Name} is not tracking {backupObject.RelativePath}");

        _backupObjects.Remove(backupObject);
    }

    public RestorePoint CreateRestorePoint(DateTime dateTime)
    {
        var id = Guid.NewGuid();
        Repository.CreateDirectoryFromRoot(Repository.PathCombine(Name, id.ToString()));
        IStorage storage = Algorithm.CreateStorage(_backupObjects.Select(obj => obj.Repository.OpenByPathFromRoot(obj.RelativePath)), Repository, Repository.PathCombine(Name, id.ToString()));
        RestorePoint restorePoint = new (storage, _backupObjects.ToList(), dateTime, id);
        _backup.AddRestorePoint(restorePoint);

        return restorePoint;
    }
}