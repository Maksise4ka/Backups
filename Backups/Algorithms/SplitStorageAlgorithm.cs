using Backups.Interfaces;
using Backups.Interfaces.Composite;
using Backups.Storages;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm<TArchivator> : IAlgorithm
    where TArchivator : IArchivator
{
    private readonly TArchivator _archivator;

    public SplitStorageAlgorithm(TArchivator archivator)
    {
        _archivator = archivator;
    }

    public IStorage CreateStorage(IEnumerable<IRepositoryObject> forest, IRepository repository, string directoryToSave)
    {
        List<IStorage> storages = new ();
        foreach (IRepositoryObject tree in forest)
        {
            string filePath = $"{repository.PathCombine(directoryToSave, Guid.NewGuid().ToString())}.{_archivator.Extension}";
            Stream stream = repository.CreateFileFromRoot(filePath);
            storages.Add(_archivator.CreateArchive(new List<IRepositoryObject>() { tree }, repository, filePath, stream));
        }

        return new SplitStorageAdapter(storages);
    }

    public override string ToString() => "SplitStorageAlgorithn<{_archivator}>";
}