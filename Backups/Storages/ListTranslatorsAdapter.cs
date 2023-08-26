using Backups.Interfaces;
using Backups.Interfaces.Composite;

namespace Backups.Storages;

public class ListTranslatorsAdapter : IObjectTranslator
{
    public ListTranslatorsAdapter(IEnumerable<IObjectTranslator> objectTranslators)
    {
        Translators = objectTranslators;
    }

    public IEnumerable<IObjectTranslator> Translators { get; }

    public IEnumerable<IRepositoryObject> GetRepositoryObjects() => Translators.SelectMany(trans => trans.GetRepositoryObjects());

    public void Dispose()
    {
        foreach (IObjectTranslator translator in Translators)
            translator.Dispose();
    }
}