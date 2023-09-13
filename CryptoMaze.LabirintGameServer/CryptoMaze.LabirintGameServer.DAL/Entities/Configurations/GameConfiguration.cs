using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.FinishTime).IsRequired(false);
            builder.Property(x => x.BtcBlocksCollected).IsRequired();
            builder.Property(x => x.EthBlocksCollected).IsRequired();
            builder.Property(x => x.TonBlocksCollected).IsRequired();
            builder.Property(x => x.EnergyCollected).IsRequired();
            builder.Property(x => x.CryptoKeyFragmentsCollected).IsRequired();
            builder.Property(x => x.TimeFreezeUsed).IsRequired();
            builder.Property(x => x.CryptoKeyUsed).IsRequired();
            builder.Property(x => x.EnergySpent).IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Games).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
