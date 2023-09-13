using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class SeasonHistoryConfiguration : IEntityTypeConfiguration<SeasonHistory>
    {
        public void Configure(EntityTypeBuilder<SeasonHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rank).IsRequired();
            builder.Property(x => x.Score).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(64);

            builder.Property(x => x.SeasonId).IsRequired();
            builder.Property(x => x.UserId).IsRequired(false);

            builder.HasIndex(x => new { x.UserId, x.SeasonId, x.Type }).IsUnique();
            builder.HasIndex(x => new { x.Type, x.Rank }).IsUnique();

            builder.HasOne(x => x.Season).WithMany(x => x.History).HasForeignKey(x => x.SeasonId);
            builder.HasOne(x => x.User).WithMany(x => x.SeasonHistory).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
