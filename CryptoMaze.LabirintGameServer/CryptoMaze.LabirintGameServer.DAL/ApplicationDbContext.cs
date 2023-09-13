using CryptoMaze.LabirintGameServer.DAL.Entities;
using CryptoMaze.LabirintGameServer.DAL.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoMaze.LabirintGameServer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Labirint> Labirints { get; set; }
        public virtual DbSet<LabirintCryptoBlock> LabirintCryptoBlocks { get; set; }
        public virtual DbSet<LabirintEnergy> LabirintEnergies { get; set; }
        public virtual DbSet<LabirintCryptoKeyFragment> LabirintCryptoKeyFragments { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<SeasonHistory> SeasonHistory { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ShopProposal> ShopProposals { get; set; }
        public virtual DbSet<UserItem> UserItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfiguration(new UserConfiguration())
                .ApplyConfiguration(new GameConfiguration())
                .ApplyConfiguration(new LabirintConfiguration())
                .ApplyConfiguration(new LabirintCryptoBlockConfiguration())
                .ApplyConfiguration(new LabirintEnergyConfiguration())
                .ApplyConfiguration(new LabirintCryptoKeyFragmentConfiguration())
                .ApplyConfiguration(new SeasonConfiguration())
                .ApplyConfiguration(new SeasonHistoryConfiguration())
                .ApplyConfiguration(new ItemConfiguration())
                .ApplyConfiguration(new ShopProposalConfiguration())
                .ApplyConfiguration(new UserItemConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_configuration.GetConnectionString("gameServer"));
    }
}