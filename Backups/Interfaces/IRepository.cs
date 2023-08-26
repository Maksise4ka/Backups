using Backups.Interfaces.Composite;

namespace Backups.Interfaces;

public interface IRepository
{
    IRepositoryObject OpenByPathFromRoot(string pathFromRoot);

    bool ObjectExists(string pathFromRoot);

    Stream CreateFileFromRoot(string relativePath);

    void CreateDirectoryFromRoot(string relativePath);

    string PathCombine(string path1, string path2);

    void DeleteObjectFromRoot(string relativePath);

    string GetObjectName(string relativePath);

    string GetDirName(string relativePath);
}