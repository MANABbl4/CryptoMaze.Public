namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class User : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool NameChanged { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<SeasonHistory> SeasonHistory { get; set; }
        public ICollection<UserItem> Items { get; set; }
    }
}
