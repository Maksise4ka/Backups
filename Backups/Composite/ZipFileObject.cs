using System.IO.Compression;
using Backups.Interfaces.Composite;

namespace Backups.Composite;

public class ZipFileObject : IZipObject
{
    public ZipFileObject(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public IRepositoryObject ToRepObject(ZipArchiveEntry entry)
        => new FileObject(entry.Name, entry.Open);
}