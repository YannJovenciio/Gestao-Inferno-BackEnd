using Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Adapters.Outbound.Persistence.Repositories.image
{
    public class ImageRepository : IImageRepository
    {
        private readonly HellDbContext _context;

        public ImageRepository(HellDbContext context)
        {
            _context = context;
        }

        public async Task<Image> UploadImageAsync(Image input)
        {
            await _context.Image.AddAsync(input);
            await _context.SaveChangesAsync();
            throw new NotImplementedException();
        }
    }
}
