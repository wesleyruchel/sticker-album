namespace APIStickerAlbum.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(string base64File, string fileName);
}
