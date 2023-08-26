using Backups.Interfaces.Composite;

namespace Backups.Interfaces;

public interface IObjectTranslator : IDisposable
{
    IEnumerable<IRepositoryObject> GetRepositoryObjects();
}