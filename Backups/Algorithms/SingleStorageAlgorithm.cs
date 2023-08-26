using Backups.Interfaces;
using Backups.Interfaces.Composite;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm<TArchivator> : IAlgorithm
    where TArchivator : IArchivator
{
    private readonly TArchivator _archivator;

    public SingleStorageAlgorithm(TArchivator archivator)
    {
        _archivator = archivator;
    }

    public IStorage CreateStorage(IEnumerable<IRepositoryObject> forest, IRepository repository, string directoryToSave)
    {
        string filePath = $"{repository.PathCombine(directoryToSave, Guid.NewGuid().ToString())}.{_archivator.Extension}";
        Stream stream = repository.CreateFileFromRoot(filePath);
        IStorage storage = _archivator.CreateArchive(forest, repository, filePath, stream);

        return storage;
    }

    public override string ToString() => $"SingleStorageAlgorithn<{_archivator}>";
}