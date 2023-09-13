namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class LabirintEnergy : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public bool Found { get; set; }
        public bool Storage { get; set; }

        public int LabirintId { get; set; }
        public Labirint Labirint { get; set; }
    }
}
