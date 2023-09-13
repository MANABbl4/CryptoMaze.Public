using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.NameChanged).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
