using Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Adapters.Outbound.Persistence.Repositories.image;

public interface IImageRepository
{
    Task<Image> UploadImageAsync(Image input);
}
