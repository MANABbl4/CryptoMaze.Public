using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CryptoMaze.Common;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class ShopProposalConfiguration : IEntityTypeConfiguration<ShopProposal>
    {
        public void Configure(EntityTypeBuilder<ShopProposal> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BuyItemAmount).IsRequired();
            builder.Property(x => x.BuyShopItemType).IsRequired();
            builder.Property(x => x.SellItemAmount).IsRequired();
            builder.Property(x => x.SellShopItemType).IsRequired();
            builder.Property(x => x.Order).IsRequired();

            builder.HasOne(x => x.BuyItem).WithMany(x => x.BuyProposals).HasForeignKey(x => x.BuyItemId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.SellItem).WithMany(x => x.SellProposals).HasForeignKey(x => x.SellItemId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
