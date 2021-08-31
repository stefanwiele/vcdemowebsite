using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client_api_test_service_dotnet.hub
{
    public class IdentityHubService
    {
        private readonly RSA _privateKey; // used to sign the jwsCommit

        private const string KeyId = "kid";
        private const string Iss = "did:example:abc123";
        private const string Aud = "did:example:abc456";
        private const string Sub = "did:example:abc123";

        public IdentityHubService()
        {
            _privateKey = RSA.Create();
        }

        public Task<HttpResponseMessage> SendToConsumerAsync(IVerifiableCredential commitObject)
        {
            return SendToIdentityHubAsync(Configuration.ConsumerUri, commitObject);
        }

        public async Task<HttpResponseMessage> SendToIdentityHubAsync(Uri hub, IVerifiableCredential credential)
        {
            // var commit = CreateJsonCommitObject(credential, KeyId);
            //
            // var writeRequest = new WriteRequest
            // {
            //     Issuer = Iss,
            //     Audience = Aud,
            //     Subject = Sub,
            //     Commit = commit
            // };
            // var json = JsonConvert.SerializeObject(writeRequest);

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(credential);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await client.PostAsync(hub, content);
        }

        private Commit CreateJsonCommitObject(IVerifiableCredential credentials, string keyId)
        {
            var jwtHeaders = CreateJwtHeader(credentials.Type);
            var jwtHeadersJson = JsonConvert.SerializeObject(jwtHeaders);
            var jwtHeadersBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(jwtHeadersJson));

            var jwtPayloadJson = JsonConvert.SerializeObject(credentials);
            var jwtPayloadBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(jwtPayloadJson));

            var alg = new HMACSHA256(_privateKey.ExportRSAPrivateKey());
            var jwtSignature = alg.ComputeHash(Encoding.UTF8.GetBytes($"{jwtHeadersBase64}.{jwtPayloadBase64}"));
            var jwtSignatureBase64 = Base64UrlEncode(jwtSignature);

            // var jwt = $"{jwtHeadersBase64}.{jwtPayloadBase64}.{jwtSignatureBase64}";

            var contents = jwtHeadersBase64 + "." + jwtPayloadBase64;
            var encoded = Encoding.UTF8.GetBytes(contents);

            var commitHeader = new Dictionary<string, object>
            {
                { "iss", keyId },
                { "rev", Convert.ToBase64String(SHA256.Create().ComputeHash(encoded)) }
            };

            var commit = new Commit
            {
                ProtectedHeader = jwtHeadersBase64,
                Payload = jwtPayloadBase64,
                // Signature = jwtSignatureBase64, // invalid signature atm
                Header = commitHeader
            };
            return commit;
        }

        public static string Encode(object payload, byte[] keyBytes)
        {
            var segments = new List<string>();
            var alg = new HMACSHA256(keyBytes);
            var header = new { alg = "HS256", typ = "JWT" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));
            //byte[] payloadBytes = Encoding.UTF8.GetBytes(@"{"iss":"761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com","scope":"https://www.googleapis.com/auth/prediction","aud":"https://accounts.google.com/o/oauth2/token","exp":1328554385,"iat":1328550785}");

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            var stringToSign = string.Join(".", segments.ToArray());

            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = alg.ComputeHash(bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        private static Dictionary<string, object> CreateJwtHeader(string credentialType)
        {
            var parameters = new Dictionary<string, object>
            {
                { "context", "GAIA-X" },
                { "type", credentialType }, // e.g. RegistrationCredentials
                { "interface", "Collections" },
                { "operation", "create" },
                { "committed_at", DateTime.Now.ToUniversalTime().ToString("O") },
                { "commit_strategy", "basic" },
                { "sub", "sub" },
                { "alg", "HS256" },
                { "typ", "JWT" }
            };
            return parameters;
        }
    }
}