using System;

namespace client_api_test_service_dotnet
{
    public static class Configuration
    {
        public static readonly Uri ServiceUri =
            new Uri("http://20.93.231.11:8181/api/identity/dids");

        public static readonly Uri ConsumerUri =
            new Uri("http://20.103.169.20:8181/api/identity-hub/collections-commit");
            
        public static readonly Uri LocalhostConnectorUri =
            new Uri("http://localhost:8181/api/identity-hub/collections-commit");

        public static readonly Uri ProviderUri =
            new Uri("http://20.103.136.133:8181/api/identity-hub/collections-commit");

        public const string HackathonServiceName = "LinkedDomains";
    }
}