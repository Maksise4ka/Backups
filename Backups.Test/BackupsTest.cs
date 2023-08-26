using System.Text;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Models;
using Backups.Storages;
using Xunit;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTest
{
    private readonly InMemoryRepository repository;
    private BackupObject obj1;
    private BackupObject obj2;

    public BackupsTest()
    {
        MemoryFileSystem fs = new ();
        fs.CreateDirectory("/mnt/c/dir1/");

        repository = new InMemoryRepository("/mnt/c/", fs);

        byte[] bytes1 = Encoding.ASCII.GetBytes("Oh purple flame \nO whirling whell of life\nAppear, Move, Rise\n Come onto me Bartzabel");
        MemoryStream file1 = new (bytes1);
        byte[] bytes2 = Encoding.ASCII.GetBytes("Меня зовут Кира Йошикагэ. Мне 33 года. Мой дом находится в северо-восточной части Морио, в районе поместий.");
        MemoryStream file2 = new (bytes2);
        byte[] bytes3 = Encoding.ASCII.GetBytes("Это Реквием. То, что ты видишь, реально.");
        MemoryStream file3 = new (bytes3);

        using Stream memoryStream1 = fs.OpenFile("/mnt/c/file1.txt", FileMode.Create, FileAccess.ReadWrite);
        file1.CopyTo(memoryStream1);
        using Stream memoryStream2 = fs.OpenFile("/mnt/c/dir1/file2.txt", FileMode.Create, FileAccess.ReadWrite);
        file2.CopyTo(memoryStream2);
        using Stream memoryStream3 = fs.OpenFile("/mnt/c/dir1/file3.txt", FileMode.Create, FileAccess.ReadWrite);
        file3.CopyTo(memoryStream3);

        obj1 = new ("file1.txt", repository);
        obj2 = new ("dir1/", repository);
    }

    [Fact]
    public void InMemoryRepository_CheckRestorePointsAndStorages()
    {
        Backup backup = new ();
        BackupTask task = new ("task", repository, backup, new SplitStorageAlgorithm<ZipArchivator>(new ZipArchivator()));
        task.AddBackupObject(obj1);
        task.AddBackupObject(obj2);

        RestorePoint restorePoint1 = task.CreateRestorePoint(DateTime.Now);

        IObjectTranslator translator1 = restorePoint1.Storage.ObjectTranslator;
        Assert.Equal(2, translator1.GetRepositoryObjects().Count());
        translator1.Dispose();

        task.RemoveBackupObject(obj1);

        RestorePoint restorePoint2 = task.CreateRestorePoint(DateTime.Now);
        using IObjectTranslator translator2 = restorePoint2.Storage.ObjectTranslator;

        Assert.Single(translator2.GetRepositoryObjects());
        Assert.Equal(2, backup.RestorePoints.Count());
    }

    [Fact]
    public void InMemoryRepository_CheckDirecotiriesAndFiles()
    {
        BackupTask task = new ("task", repository, new Backup(), new SingleStorageAlgorithm<ZipArchivator>(new ZipArchivator()));
        task.AddBackupObject(obj1);
        task.AddBackupObject(obj2);

        RestorePoint restorePoint = task.CreateRestorePoint(DateTime.Now);
        ZipStorage storage = (restorePoint.Storage as ZipStorage) !;

        FileSystem fileSystem = repository.FileSystem;

        Assert.True(fileSystem.DirectoryExists($@"/mnt/c/task/{restorePoint.Id}"));
        Assert.True(fileSystem.FileExists($@"/mnt/c/{storage.Path}"));
    }
}