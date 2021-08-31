using System.Collections.Generic;
using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public class Commit
    {
        [JsonConstructor]
        public Commit()
        {
        }

        [JsonProperty("protected")]
        public string ProtectedHeader { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("header")]
        public IDictionary<string, object> Header { get; set; }
    }
}