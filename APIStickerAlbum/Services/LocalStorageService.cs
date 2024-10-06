using APIStickerAlbum.Interfaces;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace APIStickerAlbum.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _storagePath;

    public LocalStorageService(IConfiguration _config)
    {
        _storagePath = _config["Storage:LocalStorage:Path"]!;

        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<string> UploadFileAsync(string base64File, string fileName)
    {
        byte[] fileBytes = Convert.FromBase64String(base64File);

        using var inputStrem = new MemoryStream(fileBytes);
        using var image = await Image.LoadAsync(inputStrem);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(800, 600)
        }));

        var jpegEncoder = new JpegEncoder
        {
            Quality = 75
        };

        var filePath = Path.Combine(_storagePath, fileName);

        await image.SaveAsync(filePath);

        return filePath;
    }
}
