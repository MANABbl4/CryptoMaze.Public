using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Type).IsRequired();

            builder.HasIndex(x => x.Type).IsUnique();
        }
    }
}
