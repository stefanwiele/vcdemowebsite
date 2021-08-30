using System.IO;
using System.Linq;
using System.Reflection;

namespace client_api_test_service_dotnet_tests.Resources
{
    public class RSA
    {
        public static readonly string Public = ReadKeyFile("key_rsa.pub.pem");
        public static readonly string Private = ReadKeyFile("key_rsa.pem");

        private static string ReadKeyFile(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = name;
            if (!name.StartsWith(nameof(RSA)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            using var stream = assembly.GetManifestResourceStream(resourcePath);

            // var streamBuffer = new byte[stream.Length];
            // stream.Read(streamBuffer, 0, streamBuffer.Length);
            // return streamBuffer;

            var length = stream.Length;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd()
                .Replace("\n", "")
                .Replace("-----BEGIN OPENSSH PRIVATE KEY-----", "")
                .Replace("-----END OPENSSH PRIVATE KEY-----", "");
        }
    }
}