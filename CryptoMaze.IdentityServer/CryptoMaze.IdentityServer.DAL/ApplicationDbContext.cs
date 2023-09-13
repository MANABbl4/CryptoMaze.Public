using CryptoMaze.IdentityServer.DAL.Entities;
using CryptoMaze.IdentityServer.DAL.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CryptoMaze.IdentityServer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLoginCode> UserLoginCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfiguration(new UserConfiguration())
                .ApplyConfiguration(new UserLoginCodeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_configuration.GetConnectionString("identity"));
    }
}