using System.Collections.Generic;
using Newtonsoft.Json;

namespace client_api_test_service_dotnet.did
{
    public class DidDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("publicKey")]
        public DidDocumentPublicKey PublicKey { get; set; }

        [JsonProperty("service")]
        public List<DiDDocumentService> Services { get; set; }
    }
}