using System.Text.Json.Serialization;

namespace MyApiProject.Models
{
    public class TokenData
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        //public string ExpiresIn { get; set; }
        public int ExpiresIn { get; set; } = int.MinValue;

        [JsonPropertyName("refresh_expires_in")]
        //public string RefreshExpiresIn { get; set; }
        public int RefreshExpiresIn { get; set; } = int.MinValue;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

       
    }

    //public class TokenResponse
    //{
    //    [JsonPropertyName("status")]
    //    public string Status { get; set; }

    //    [JsonPropertyName("message")]
    //    public string Message { get; set; }

    //    [JsonPropertyName("data")]
    //    public TokenData Data { get; set; }
    //}
}
