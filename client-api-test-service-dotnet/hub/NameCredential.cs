using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public class NamedCredential : IVerifiableCredential
    {
        [JsonIgnore]
        public string Type => "NamedCredential";

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}