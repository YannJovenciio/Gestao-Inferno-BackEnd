using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Configuration.Soul
{
    public class SoulConfiguration : IEntityTypeConfiguration<Entity.Soul>
    {
        public void Configure(EntityTypeBuilder<Entity.Soul> builder)
        {
            builder.HasKey(s => s.IdSoul);
            builder.Property(s => s.Description).IsRequired().HasMaxLength(500);
            builder.Property(s => s.Level).IsRequired();
            builder.Property(s => s.SoulName).IsRequired().HasMaxLength(100);

            builder
                .HasOne(s => s.Cavern)
                .WithMany(c => c.Souls)
                .HasForeignKey(s => s.CavernId)
                .IsRequired(false);
        }
    }
}
