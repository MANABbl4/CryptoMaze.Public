namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public interface IEntity<T>
        where T : struct
    {
        T Id { get; }
    }
}
