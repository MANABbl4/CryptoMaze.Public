using System.ComponentModel.DataAnnotations;

namespace CryptoMaze.ClientServer.Authentication.Requests
{
    public class RefreshTokenRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string AccessToken { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string RefreshToken { get; set; }
    }
}
