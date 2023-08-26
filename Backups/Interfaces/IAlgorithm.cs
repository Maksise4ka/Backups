using Backups.Interfaces.Composite;

namespace Backups.Interfaces;

public interface IAlgorithm
{
    IStorage CreateStorage(IEnumerable<IRepositoryObject> forest, IRepository repository, string directoryToSave);
}