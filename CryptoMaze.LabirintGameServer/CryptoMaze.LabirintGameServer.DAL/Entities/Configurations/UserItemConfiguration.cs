using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class UserItemConfiguration : IEntityTypeConfiguration<UserItem>
    {
        public void Configure(EntityTypeBuilder<UserItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemAmount).IsRequired();

            builder.HasIndex(x => new { x.ItemId, x.UserId }).IsUnique();

            builder.HasOne(x => x.Item).WithMany(x => x.UserItems).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.User).WithMany(x => x.Items).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
