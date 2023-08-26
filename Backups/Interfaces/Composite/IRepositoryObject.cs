namespace Backups.Interfaces.Composite;

public interface IRepositoryObject
{
    string Name { get; }

    void Accept(IVisitor visitor);
}