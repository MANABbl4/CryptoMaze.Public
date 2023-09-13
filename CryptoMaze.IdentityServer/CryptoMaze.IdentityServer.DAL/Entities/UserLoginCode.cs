namespace CryptoMaze.IdentityServer.DAL.Entities
{
    public sealed class UserLoginCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiry { get; set; }
        public bool Used { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
