namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class SeasonHistory : IEntity<int>
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
        public Common.CryptoType Type { get; set; }
        public string UserName { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
