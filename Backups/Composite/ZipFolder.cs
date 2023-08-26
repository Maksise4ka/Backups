using System.IO.Compression;
using Backups.Interfaces.Composite;

namespace Backups.Composite;

public class ZipFolder : IZipObject
{
    public ZipFolder(string name, IEnumerable<IZipObject> children)
    {
        Name = name;
        Children = children;
    }

    public string Name { get; }

    public IEnumerable<IZipObject> Children { get; }

    public IRepositoryObject ToRepObject(ZipArchiveEntry entry)
    {
        IEnumerable<IRepositoryObject> Func()
        {
            var arch = new ZipArchive(entry.Open(), ZipArchiveMode.Read);
            var res = new List<IRepositoryObject>();
            foreach (ZipArchiveEntry child in arch.Entries)
            {
                IZipObject obj = Children.First(x => x.Name.Equals(child.Name));
                res.Add(obj.ToRepObject(child));
            }

            return res;
        }

        return new DirectoryObject(entry.Name[..^4], Func);
    }
}