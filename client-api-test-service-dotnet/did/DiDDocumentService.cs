using Newtonsoft.Json;

namespace client_api_test_service_dotnet.did
{
    public class DiDDocumentService
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("serviceEndpoint")]
        public string Endpoint { get; set; }
    }
}