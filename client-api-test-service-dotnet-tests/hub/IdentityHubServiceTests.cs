using System;
using System.Threading.Tasks;
using client_api_test_service_dotnet;
using client_api_test_service_dotnet.hub;
using NUnit.Framework;

namespace client_api_test_service_dotnet_tests.hub
{
    [TestFixture]
    public class IdentityHubServiceTests
    {
        private IdentityHubService _identityHubService;

        [SetUp]
        public void SetUp()
        {
            _identityHubService = new IdentityHubService();
        }

        [Test]
        public async Task MyTest()
        {
            var hub = Configuration.LocalhostConnectorUri;
            // var credentials = new GaiaXCredentials { Region = "eu", CompanyId = "Consumer" };
            var credentials = new NameCredential { FirstName = "John", LastName = "Doe" };
            var response = await _identityHubService.SendToIdentityHubAsync(hub, credentials);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);
        }
    }
}