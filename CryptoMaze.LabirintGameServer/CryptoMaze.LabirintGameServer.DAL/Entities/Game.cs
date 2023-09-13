namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class Game : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int EnergySpent { get; set; }
        public int BtcBlocksCollected { get; set; }
        public int EthBlocksCollected { get; set; }
        public int TonBlocksCollected { get; set; }
        public int EnergyCollected { get; set; }
        public int CryptoKeyFragmentsCollected { get; set; }
        public bool TimeFreezeUsed { get; set; }
        public bool CryptoKeyUsed { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Labirint> Labirints { get; set; }
    }
}
