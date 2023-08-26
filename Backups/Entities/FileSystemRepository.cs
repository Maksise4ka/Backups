using Backups.Composite;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Interfaces.Composite;

namespace Backups.Entities;

public class FileSystemRepository : IRepository
{
    public FileSystemRepository(string rootPath)
    {
        RootPath = rootPath;

        Directory.CreateDirectory(RootPath);
    }

    public string RootPath { get; }

    public string PathCombine(string path1, string path2) => Path.Combine(path1, path2);

    public void CreateDirectoryFromRoot(string relativePath)
    {
        Directory.CreateDirectory($@"{RootPath}\{relativePath}");
    }

    public Stream CreateFileFromRoot(string relativePath)
    {
        return File.Open($@"{RootPath}\{relativePath}", FileMode.Create, FileAccess.Write);
    }

    public bool ObjectExists(string pathFromRoot)
    {
        string absolutePath = $@"{RootPath}\{pathFromRoot}";
        return File.Exists(absolutePath) || Directory.Exists(absolutePath);
    }

    public IRepositoryObject OpenByPathFromRoot(string pathFromRoot)
    {
        string absolutePath;
        if (pathFromRoot.EndsWith('\\'))
            absolutePath = $@"{RootPath}\{pathFromRoot[.. (pathFromRoot.Length - 1)]}";
        else
            absolutePath = $@"{RootPath}\{pathFromRoot}";

        if (File.Exists(absolutePath))
        {
            return new FileObject(
                Path.GetFileName(absolutePath),
                new Func<Stream>(() => File.OpenRead(absolutePath)));
        }
        else if (Directory.Exists(absolutePath))
        {
            return new DirectoryObject(
                Path.GetFileName(absolutePath),
                new Func<IEnumerable<IRepositoryObject>>(
                    () => Directory.EnumerateFileSystemEntries(absolutePath).Select(str => OpenByPathFromRoot(str[RootPath.Length..]))));
        }
        else
        {
            throw new BackupObjectException(absolutePath);
        }
    }

    public void DeleteObjectFromRoot(string relativePath)
    {
        string fullPath = $@"{RootPath}\{relativePath}";
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        else if (Directory.Exists(fullPath))
            Directory.Delete(fullPath, true);
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