using System.ComponentModel.DataAnnotations;

namespace CryptoMaze.ClientServer.Game.Requests
{
    public class BuyRequest
    {
        [Required]
        public int ProposalId { get; set; }
    }
}
