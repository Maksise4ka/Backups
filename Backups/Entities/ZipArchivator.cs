using System.IO.Compression;
using Backups.Composite;
using Backups.Interfaces;
using Backups.Interfaces.Composite;
using Backups.Storages;

namespace Backups.Entities;

public class ZipArchivator : IArchivator
{
    public string Extension => "zip";

    public IStorage CreateArchive(IEnumerable<IRepositoryObject> forest, IRepository repository, string filePath, Stream stream)
    {
        using ZipArchive archive = new (stream, ZipArchiveMode.Create);
        ZipVisitor visitor = new (archive);

        foreach (IRepositoryObject tree in forest)
        {
            tree.Accept(visitor);
        }

        return new ZipStorage(new ZipFolder(Guid.NewGuid().ToString(), visitor.Forest), filePath, repository);
    }

    public override string ToString() => "ZipArchivator";
}