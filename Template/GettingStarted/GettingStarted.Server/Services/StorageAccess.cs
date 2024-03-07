using Azure.Storage.Blobs;
using Codeer.LowCode.Blazor.SystemSettings;

namespace GettingStarted.Server.Services
{
  public class StorageAccess
  {
    public static async Task<MemoryStream> ReadFileAsync(Guid? guid, string storageName)
    {
      if (guid == null) throw new Exception();

      var storage = SystemConfig.Instance.FileStorages.Where(e => e.Name == storageName).FirstOrDefault()!;

      if (storage.FileStorageType == FileStorageType.AzureBlobStorage)
      {
        BlobContainerClient container = new(storage.ConnectionString, storage.ContainerName);
        var blobClient = container.GetBlobClient($"{guid!.Value}");
        var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        var bin = memoryStream.ToArray();
        memoryStream.Position = 0;
        return memoryStream;
      }
      else if (storage.FileStorageType == FileStorageType.FileSystem)
      {
        if (string.IsNullOrEmpty(storage.Directory)) throw new Exception();
        var path = Path.Combine(storage.Directory, guid.ToString()!);
        return new MemoryStream(File.ReadAllBytes(path));
      }

      throw new Exception();
    }

    public static async Task DeleteFiles(string storageName, Guid[] files)
    {
      var storage = SystemConfig.Instance.FileStorages.Where(e => e.Name == storageName).FirstOrDefault()!;
      foreach (var file in files)
      {
        try
        {
          if (storage.FileStorageType == FileStorageType.AzureBlobStorage)
          {
            BlobContainerClient container = new(storage.ConnectionString, storage.ContainerName);
            var blobClient = container.GetBlobClient($"{file}");
            await blobClient.DeleteIfExistsAsync();
          }
          else if (storage.FileStorageType == FileStorageType.FileSystem)
          {
            if (string.IsNullOrEmpty(storage.Directory)) throw new Exception();
            var path = Path.Combine(storage.Directory, file.ToString()!);
            File.Delete(path);
          }
        }
        catch { }
      }
    }

    public static async Task WriteFile(string? storageName, Guid guid, MemoryStream memoryStream)
    {
      var storage = SystemConfig.Instance.FileStorages.Where(e => e.Name == storageName).FirstOrDefault();
      if (storage == null) throw new Exception();

      if (storage.FileStorageType == FileStorageType.AzureBlobStorage)
      {
        BlobContainerClient container = new(storage.ConnectionString, storage.ContainerName);
        var blobClient = container.GetBlobClient($"{guid}");
        await blobClient.UploadAsync(memoryStream, true);
      }
      else if (storage.FileStorageType == FileStorageType.FileSystem)
      {
        if (string.IsNullOrEmpty(storage.Directory)) throw new Exception();
        Directory.CreateDirectory(storage.Directory);
        var path = Path.Combine(storage.Directory, guid.ToString());
        File.WriteAllBytes(path, memoryStream.ToArray());
      }
    }
  }
}
