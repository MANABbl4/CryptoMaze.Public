namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class Labirint : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int TimeToFinishSeconds { get; set; }
        public bool HasCryptoStorage { get; set; }
        public bool CryptoStorageOpened { get; set; }
        public bool SpeedRocketUsed { get; set; }
        public int LabirintOrderId { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public ICollection<LabirintCryptoBlock> CryptoBlocks { get; set; }
        public ICollection<LabirintEnergy> Energies { get; set; }
        public ICollection<LabirintCryptoKeyFragment> CryptoKeyFragments { get; set; }
    }
}
