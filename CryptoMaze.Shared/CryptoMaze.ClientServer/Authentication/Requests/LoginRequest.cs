using System.ComponentModel.DataAnnotations;

namespace CryptoMaze.ClientServer.Authentication.Requests
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string Code { get; set; }
    }
}
