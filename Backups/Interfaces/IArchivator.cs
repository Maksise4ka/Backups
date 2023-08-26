using Backups.Interfaces.Composite;

namespace Backups.Interfaces;

public interface IArchivator
{
    string Extension { get; }

    IStorage CreateArchive(IEnumerable<IRepositoryObject> forest, IRepository repository, string filePath, Stream stream);
}