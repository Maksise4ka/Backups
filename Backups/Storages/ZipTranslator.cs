using System.IO.Compression;
using Backups.Composite;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Interfaces.Composite;

namespace Backups.Storages;

public class ZipTranslator : IObjectTranslator
{
    private readonly ZipFolder _zipFolder;
    private Stream? _archiveStream = null;

    public ZipTranslator(ZipFolder zipFolder, IRepository repository, string path)
    {
        Repository = repository;
        Path = path;
        _zipFolder = zipFolder;
    }

    public IRepository Repository { get; }
    public string Path { get; }

    public IEnumerable<IRepositoryObject> GetRepositoryObjects()
    {
        if ((Repository.OpenByPathFromRoot(Path) as IFileObject) is null)
            throw new BackupsException("Path point to a directory");

        if (_archiveStream is null)
        {
            IFileObject file = (Repository.OpenByPathFromRoot(Path) as IFileObject) !;
            _archiveStream = file.Stream;
        }

        ZipArchive archive = new (_archiveStream, ZipArchiveMode.Read);
        List<IRepositoryObject> res = new ();
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            IZipObject obj = _zipFolder.Children.First(child => child.Name.Equals(entry.Name));
            res.Add(obj.ToRepObject(entry));
        }

        return res;
    }

    public void Dispose()
    {
        if (_archiveStream is not null)
            _archiveStream.Dispose();
    }
}