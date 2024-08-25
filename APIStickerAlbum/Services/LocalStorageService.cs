using APIStickerAlbum.Interfaces;

namespace APIStickerAlbum.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _storagePath;

    public LocalStorageService(IConfiguration _config)
    {
        _storagePath = _config["LocalStorage:Path"]!;

        if (!Directory.Exists(_storagePath))
        { 
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var filePath = Path.Combine(_storagePath, fileName);

        using(var fileStreamOutput = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(fileStreamOutput);
        }

        return $"/uploads/{fileName}";
    }
}
