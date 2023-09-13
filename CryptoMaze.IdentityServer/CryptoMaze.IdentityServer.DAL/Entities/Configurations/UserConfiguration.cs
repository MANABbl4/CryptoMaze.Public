using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMaze.IdentityServer.DAL.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
            builder.Property(x => x.RegisteredDate).IsRequired();
            builder.Property(x => x.RefreshToken).IsRequired(false).HasMaxLength(512);
            builder.Property(x => x.RefreshTokenExpiration).IsRequired(false);

            builder.HasIndex(x => x.RefreshToken);
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
