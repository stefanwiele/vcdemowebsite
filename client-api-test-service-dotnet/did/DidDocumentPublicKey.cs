using Newtonsoft.Json;

namespace client_api_test_service_dotnet.did
{
    public class DidDocumentPublicKey
    {
        [JsonProperty("id")]
        public string Id { get; set; } // DID

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; } // DID

        [JsonProperty("publicKeyRem")]
        public string PublicRem { get; set; }
    }
}