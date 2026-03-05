using System.ComponentModel.DataAnnotations;

namespace Inferno.src.Adapters.Models;

public record ImageUpload
{
    [Required]
    public string FileName { get; init; } = string.Empty;
    public Byte[] ImageData { get; init; } = [];
}
