using Newtonsoft.Json;

namespace STC.Models
{
    public class UserToken
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
