namespace Backups.Interfaces.Composite;

public interface IFileObject : IRepositoryObject
{
    Stream Stream { get; }
}