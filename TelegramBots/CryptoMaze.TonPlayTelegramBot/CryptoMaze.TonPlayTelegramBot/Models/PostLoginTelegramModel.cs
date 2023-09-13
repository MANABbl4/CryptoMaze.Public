using System.Text.Json.Serialization;

namespace CryptoMaze.TonPlayTelegramBot.Models
{
    public class PostLoginTelegramModel
    {
        [JsonPropertyName("id")]
        public long id { get; set; }

        [JsonPropertyName("username")]
        public string username { get; set; }

        [JsonPropertyName("first_name")]
        public string first_name { get; set; }

        [JsonPropertyName("last_name")]
        public string last_name { get; set; }

        [JsonPropertyName("locale")]
        public string locale { get; set; }

        [JsonPropertyName("hash")]
        public string hash { get; set; }

        [JsonPropertyName("bot_key")]
        public string bot_key { get; set; }
    }
}
