using Backups.Composite;
using Backups.Interfaces;

namespace Backups.Storages;

public class ZipStorage : IStorage
{
    private readonly ZipFolder _zipFolder;

    public ZipStorage(ZipFolder zipFolder, string path, IRepository repository)
    {
        _zipFolder = zipFolder;
        Path = path;
        Repository = repository;
    }

    public string Path { get; }
    public IRepository Repository { get; }

    public IObjectTranslator ObjectTranslator => new ZipTranslator(_zipFolder, Repository, Path);
}