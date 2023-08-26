using System.IO.Compression;
using Backups.Composite;
using Backups.Interfaces.Composite;

namespace Backups.Entities;

public class ZipVisitor : IVisitor
{
    private readonly Stack<ZipArchive> _stack;
    private readonly Stack<List<IZipObject>> _zipStack;

    public ZipVisitor(ZipArchive archive)
    {
        _stack = new Stack<ZipArchive>();
        _stack.Push(archive);
        _zipStack = new Stack<List<IZipObject>>();

        _zipStack.Push(new List<IZipObject>());
    }

    public IEnumerable<IZipObject> Forest => _zipStack.Peek();

    public void Visit(IFileObject fileObject)
    {
        ZipArchiveEntry newZip = _stack.Peek().CreateEntry($"{fileObject.Name}");
        using Stream stream = newZip.Open();
        using Stream filestream = fileObject.Stream;
        filestream.CopyTo(stream);

        ZipFileObject zipFile = new ($"{fileObject.Name}");
        _zipStack.Peek().Add(zipFile);
    }

    public void Visit(IDirectoryObject directoryObject)
    {
        Stream stream = _stack.Peek().CreateEntry($"{directoryObject.Name}.zip").Open();
        using var newZip = new ZipArchive(stream, ZipArchiveMode.Create);
        _stack.Push(newZip);

        _zipStack.Push(new List<IZipObject>());
        foreach (IRepositoryObject child in directoryObject.Children)
        {
            child.Accept(this);
        }

        ZipFolder zipFolder = new ($"{directoryObject.Name}.zip", _zipStack.Pop());
        _zipStack.Peek().Add(zipFolder);
        _stack.Pop();
    }
}