using Backups.Interfaces.Composite;

namespace Backups.Composite;

public class FileObject : IFileObject
{
    private readonly Func<Stream> _func;

    public FileObject(string name, Func<Stream> func)
    {
        Name = name;
        _func = func;
    }

    public string Name { get; }

    public Stream Stream => _func();

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}