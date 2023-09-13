namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public class UserItem : IEntity<int>
    {
        public int Id { get; set; }
        public int ItemAmount { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
