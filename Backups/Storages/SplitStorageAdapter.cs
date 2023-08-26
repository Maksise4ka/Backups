using Backups.Interfaces;

namespace Backups.Storages;

public class SplitStorageAdapter : IStorage
{
    public SplitStorageAdapter(IEnumerable<IStorage> storages)
    {
        Storages = storages;
    }

    public IEnumerable<IStorage> Storages { get; }

    public IObjectTranslator ObjectTranslator => new ListTranslatorsAdapter(GetInterlayer());

    private IEnumerable<IObjectTranslator> GetInterlayer()
    {
        var objectTranslators = new List<IObjectTranslator>();
        foreach (IStorage storage in Storages)
        {
            objectTranslators.Add(storage.ObjectTranslator);
        }

        return objectTranslators;
    }
}