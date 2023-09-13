namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class Season : IEntity<int>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool Finished { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public ICollection<SeasonHistory> History { get; set; }
    }
}
