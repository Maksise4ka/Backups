using System.IO.Compression;

namespace Backups.Interfaces.Composite;

public interface IZipObject
{
    string Name { get; }

    IRepositoryObject ToRepObject(ZipArchiveEntry entry);
}