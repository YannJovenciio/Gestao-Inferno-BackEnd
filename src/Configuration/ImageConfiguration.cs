using Inferno.src.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inferno.src.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.IdImage);
        builder.Property(i => i.FileName);
        builder.Property(i => i.ContentType);
        builder.Property(i => i.ImageData);
        builder.Property(i => i.UploadDate);
    }
}
