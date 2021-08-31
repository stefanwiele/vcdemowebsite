using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public class GaiaXCredentials : IVerifiableCredential
    {
        [JsonIgnore]
        public string Type => "GaiaXCredentials";

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("company-id")]
        public string CompanyId { get; set; }
    }
}