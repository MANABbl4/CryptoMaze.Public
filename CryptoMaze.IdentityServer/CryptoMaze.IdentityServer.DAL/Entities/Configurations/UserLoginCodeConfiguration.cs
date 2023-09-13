using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.IdentityServer.DAL.Entities.Configurations
{
    public class UserLoginCodeConfiguration : IEntityTypeConfiguration<UserLoginCode>
    {
        public void Configure(EntityTypeBuilder<UserLoginCode> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(6);
            builder.Property(x => x.Created).IsRequired();
            builder.Property(x => x.Expiry).IsRequired();
            builder.Property(x => x.Used).IsRequired();

            builder.HasIndex(x => x.Code);
            builder.HasIndex(x => x.Created);
            builder.HasIndex(x => x.Expiry);

            builder.HasOne(x => x.User).WithMany(x => x.UserLoginCodes).HasForeignKey(x => x.UserId);
        }
    }
}
