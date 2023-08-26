namespace Backups.Interfaces.Composite;

public interface IVisitor
{
    void Visit(IFileObject fileObject);
    void Visit(IDirectoryObject directoryObject);
}