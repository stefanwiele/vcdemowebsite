using System;
using System.Threading.Tasks;
using client_api_test_service_dotnet.did;
using NUnit.Framework;

namespace client_api_test_service_dotnet_tests.did
{
    [TestFixture]
    public class DidResolverTests
    {
        private DidResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _resolver = new DidResolver();
        }

        [Test]
        public async Task MyTest()
        {
            await _resolver.GetIdentityHubEndpoint("debug-me");
        }
    }
}