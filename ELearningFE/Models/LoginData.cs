using Newtonsoft.Json;

namespace ELearningFE.Models
{
    public class LoginData
    {
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }

        [JsonProperty("userName")]
        public string? UserName { get; set; }
    }
}
