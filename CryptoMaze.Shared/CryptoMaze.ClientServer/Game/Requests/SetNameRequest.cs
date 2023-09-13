using System.ComponentModel.DataAnnotations;

namespace CryptoMaze.ClientServer.Game.Requests
{
    public class SetNameRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 24, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only letters and digits are allowed.")]
        public string Name { get; set; }
    }
}
