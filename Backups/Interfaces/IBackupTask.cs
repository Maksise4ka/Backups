using Backups.Models;

namespace Backups.Interfaces;

public interface IBackupTask
{
    string TaskPath { get; }

    void AddBackupObject(BackupObject backupObject);

    void RemoveBackupObject(BackupObject backupObject);

    RestorePoint CreateRestorePoint(DateTime dateTime);
}