using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class LabirintConfiguration : IEntityTypeConfiguration<Labirint>
    {
        public void Configure(EntityTypeBuilder<Labirint> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StartTime).IsRequired(false);
            builder.Property(x => x.FinishTime).IsRequired(false);
            builder.Property(x => x.TimeToFinishSeconds).IsRequired();
            builder.Property(x => x.HasCryptoStorage).IsRequired();
            builder.Property(x => x.CryptoStorageOpened).IsRequired();
            builder.Property(x => x.SpeedRocketUsed).IsRequired();
            builder.Property(x => x.LabirintOrderId).IsRequired();

            builder.HasIndex(x => new { x.GameId, x.LabirintOrderId }).IsUnique();

            builder.HasOne(x => x.Game).WithMany(x => x.Labirints).HasForeignKey(x => x.GameId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
