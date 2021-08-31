using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public class WriteRequest
    {
        [JsonProperty("@type")]
        public string Type { get; set; } = "WriteRequest";

        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("aud")]
        public string Audience { get; set; }

        [JsonProperty("sub")]
        public string Subject { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("@context")]
        public string Context { get; set; } = "https://schema.identity.foundation/0.1";
    }
}