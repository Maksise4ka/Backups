using Backups.Composite;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Interfaces.Composite;
using Zio.FileSystems;

namespace Backups.Entities;

public class InMemoryRepository : IRepository
{
    public InMemoryRepository(string rootPath, MemoryFileSystem fileSystem)
    {
        RootPath = rootPath;
        FileSystem = fileSystem;

        FileSystem.CreateDirectory(RootPath);
    }

    public string RootPath { get; }
    public MemoryFileSystem FileSystem { get; }

    public void CreateDirectoryFromRoot(string relativePath)
    {
        FileSystem.CreateDirectory($@"{RootPath}\{relativePath}");
    }

    public string PathCombine(string path1, string path2) => $"{path1}/{path2}";

    public Stream CreateFileFromRoot(string relativePath)
    {
        return FileSystem.OpenFile($@"{RootPath}\{relativePath}", FileMode.Create, FileAccess.Write);
    }

    public bool ObjectExists(string pathFromRoot)
    {
        string absolutePath = $@"{RootPath}/{pathFromRoot}";
        return FileSystem.FileExists(absolutePath) || FileSystem.DirectoryExists(absolutePath);
    }

    public IRepositoryObject OpenByPathFromRoot(string pathFromRoot)
    {
        string absolutePath;
        if (pathFromRoot.EndsWith('/'))
            absolutePath = $@"{RootPath}/{pathFromRoot[.. (pathFromRoot.Length - 1)]}";
        else
            absolutePath = $@"{RootPath}/{pathFromRoot}";

        if (FileSystem.FileExists(absolutePath))
        {
            return new FileObject(
                Path.GetFileName(absolutePath),
                new Func<Stream>(() => FileSystem.OpenFile(absolutePath, FileMode.Open, FileAccess.Read)));
        }
        else if (FileSystem.DirectoryExists(absolutePath))
        {
            return new DirectoryObject(
                Path.GetFileName(absolutePath),
                new Func<IEnumerable<IRepositoryObject>>(
                    () => FileSystem.EnumerateItems(absolutePath, SearchOption.TopDirectoryOnly)
                        .Select(item => OpenByPathFromRoot(item.FullName[RootPath.Length..]))));
        }
        else
        {
            throw new BackupObjectException(absolutePath);
        }
    }

    public void DeleteObjectFromRoot(string relativePath)
    {
        string fullPath = $@"{RootPath}/{relativePath}";
        if (FileSystem.FileExists(fullPath))
            FileSystem.DeleteFile(fullPath);
        else if (FileSystem.DirectoryExists(fullPath))
            FileSystem.DeleteDirectory(fullPath, true);
        else
            throw new BackupsException($"File {relativePath} is not exists");
    }

    public string GetObjectName(string relativePath)
    {
        if (relativePath.EndsWith('\\') || relativePath.EndsWith('/'))
            return Path.GetFileName(relativePath[..^1]);
        return Path.GetFileName(relativePath);
    }

    public string GetDirName(string relativePath)
    {
        if (relativePath.EndsWith('\\') || relativePath.EndsWith('/'))
            return Path.GetDirectoryName(relativePath[..^1]) !;
        return Path.GetDirectoryName(relativePath) !;
    }
}