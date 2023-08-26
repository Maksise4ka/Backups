using Backups.Interfaces.Composite;

namespace Backups.Composite;

public class DirectoryObject : IDirectoryObject
{
    private readonly Func<IEnumerable<IRepositoryObject>> _func;
    public DirectoryObject(string name, Func<IEnumerable<IRepositoryObject>> func)
    {
        Name = name;
        _func = func;
    }

    public string Name { get;  }

    public IEnumerable<IRepositoryObject> Children => _func();

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}