using client_api_test_service_dotnet;
using client_api_test_service_dotnet_tests.Resources;
using NUnit.Framework;

namespace client_api_test_service_dotnet_tests
{
    [TestFixture]
    public class IdentityHubServiceTests
    {
        private IdentityHubService _identityHubService;

        [SetUp]
        public void SetUp()
        {
            _identityHubService = new IdentityHubService(RSA.Private, RSA.Public);
        }

        [Test]
        public void MyTest()
        {
            _identityHubService.SendToIdentityHub(@"{""region"":""eu"",""companyId"":""Consumer""}");
        }
    }
}