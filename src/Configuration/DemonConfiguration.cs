using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Configuration.Demon
{
    public class DemonConfiguration : IEntityTypeConfiguration<Entity.Demon>
    {
        public void Configure(EntityTypeBuilder<Entity.Demon> builder)
        {
            builder.ToTable(nameof(Entity.Demon));

            builder.HasKey(d => d.IdDemon);

            builder.Property(d => d.IdDemon).HasColumnName(nameof(Entity.Demon.IdDemon));
            builder
                .Property(d => d.DemonName)
                .IsRequired()
                .HasColumnName(nameof(Entity.Demon.DemonName));
            builder.Property(d => d.CreatedAt).HasColumnName(nameof(Entity.Demon.CreatedAt));
            builder
                .Property(d => d.UpdatedAt)
                .IsRequired(false)
                .HasColumnName(nameof(Entity.Demon.UpdatedAt));
            builder
                .Property(d => d.CategoryId)
                .IsRequired()
                .HasColumnName(nameof(Entity.Demon.CategoryId));

            builder.Property(d => d.ImageId).IsRequired(false);

            builder
                .HasOne(d => d.Image)
                .WithMany()
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(d => d.Category)
                .WithMany(c => c.Demons)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
