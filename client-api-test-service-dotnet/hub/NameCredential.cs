using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public class NameCredential : IVerifiableCredential
    {
        [JsonIgnore]
        public string Type => "NameCredential";

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}