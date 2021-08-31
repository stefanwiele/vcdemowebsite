using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client_api_test_service_dotnet.did
{
    public class DidResolver
    {
        public async Task<Uri> GetIdentityHubEndpoint(string did)
        {
            var client = new HttpClient();

            var result = await client.GetAsync(Configuration.ServiceUri);
            var resultText = await result.Content.ReadAsStringAsync();

            var documents = JsonConvert.DeserializeObject<List<DidDocument>>(resultText);

            var document = documents.FirstOrDefault(d => d.Id.Equals(did));
            var service = document?.Services.FirstOrDefault(s => s.Type.Equals(Configuration.HackathonServiceName));

            return service?.Endpoint == null
                ? null
                : new Uri(service.Endpoint + "/api/identity-hub/collections-commit");
        }
    }
}