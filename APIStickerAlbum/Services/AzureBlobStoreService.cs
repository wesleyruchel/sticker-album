using APIStickerAlbum.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace APIStickerAlbum.Services;

public class AzureBlobStoreService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureBlobStoreService(BlobServiceClient blobServiceClient, string containerName)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = containerName;
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

        using var outputStream = new MemoryStream();
        
        await image.SaveAsync(outputStream, jpegEncoder);
        
        outputStream.Position = 0;

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(outputStream, overwrite: true);

        return blobClient.Uri.AbsoluteUri;
    }
}
