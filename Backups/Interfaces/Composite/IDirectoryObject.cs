namespace Backups.Interfaces.Composite;

public interface IDirectoryObject : IRepositoryObject
{
    IEnumerable<IRepositoryObject> Children { get; }
}