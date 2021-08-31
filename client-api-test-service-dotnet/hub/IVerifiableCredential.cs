using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public interface IVerifiableCredential
    {
        [JsonIgnore]
        public string Type { get; }
    }
}