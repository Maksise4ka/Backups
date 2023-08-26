namespace Backups.Interfaces;

public interface IStorage
{
    IObjectTranslator ObjectTranslator { get; }
}