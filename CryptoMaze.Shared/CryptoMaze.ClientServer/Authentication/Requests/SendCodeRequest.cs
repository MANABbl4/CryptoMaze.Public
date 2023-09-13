using System.ComponentModel.DataAnnotations;

namespace CryptoMaze.ClientServer.Authentication.Requests
{
    public class SendCodeRequest
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
