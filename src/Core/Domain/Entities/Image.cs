namespace Inferno.src.Core.Domain.Entities;

public class Image
{
    public int IdImage { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public byte[] ImageData { get; private set; }
    public DateTime UploadDate { get; private set; }

    protected Image() { }

    public Image(string fileName, string contentType, byte[] imageData)
    {
        FileName = fileName;
        ContentType = contentType;
        ImageData = imageData;
        UploadDate = DateTime.UtcNow;
    }
}
