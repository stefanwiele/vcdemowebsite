using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public interface VerifiableCredential
    {
        [JsonIgnore]
        public string Type { get; }
    }
}