using CryptoMaze.ClientServer.Game.DataContainers;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class ShopDataResponse
    {
        public IEnumerable<ShopProposalModel> ShopProposals { get; set; }
    }
}
