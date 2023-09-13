namespace CryptoMaze.IdentityServer.DAL.Entities
{
    public sealed class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }

        public ICollection<UserLoginCode> UserLoginCodes { get; set; }
    }
}
