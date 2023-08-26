using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IEnumerable<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        if (!RestorePointExists(restorePoint))
            throw new BackupsException($"Restore point {restorePoint.Id} is not in this Backup");

        _restorePoints.Remove(restorePoint);
    }

    public bool RestorePointExists(RestorePoint restorePoint) => _restorePoints.Contains(restorePoint);
}