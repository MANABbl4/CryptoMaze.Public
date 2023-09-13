using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    internal class LabirintCryptoKeyFragmentConfiguration : IEntityTypeConfiguration<LabirintCryptoKeyFragment>
    {
        public void Configure(EntityTypeBuilder<LabirintCryptoKeyFragment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Found).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Storage).IsRequired();

            builder.HasOne(x => x.Labirint).WithMany(x => x.CryptoKeyFragments).HasForeignKey(x => x.LabirintId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
