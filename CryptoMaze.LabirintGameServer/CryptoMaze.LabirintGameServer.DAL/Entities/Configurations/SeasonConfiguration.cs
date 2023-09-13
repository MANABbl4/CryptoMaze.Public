using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Entities.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.FinishDate).IsRequired();
            builder.Property(x => x.Finished).IsRequired();
        }
    }
}
