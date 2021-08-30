using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Jose;

namespace client_api_test_service_dotnet
{
    public class IdentityHubService
    {
        private readonly RSACryptoServiceProvider _privateKey; // used to sign the jwsCommit

        private readonly byte[]
            _publicKey; // public key of the identity hub. used to encrypt jwe object

        public IdentityHubService(string privateKey, string identityHubPublicKey)
        {
            _privateKey =
                new X509Certificate2(Convert.FromBase64String(privateKey))
                    .PrivateKey as RSACryptoServiceProvider;
            _publicKey = Convert.FromBase64String(identityHubPublicKey);
        }

        public void SendToIdentityHub(string commitObject)
        {
            var headers = createHeader("TestCredentials");
            var jwsCommit = JWT.Encode(commitObject, _privateKey, JweAlgorithm.RSA_OAEP_256,
                JweEncryption.A256GCM,
                extraHeaders: headers);

            var keyId = "kid";
            var iss = "sub";
            var aud = "aud";
            var sub = "sub";
            var header = @"{""iss"":""kid"",""rev"":""sfMFC77qzDj8IN5mzUhIpkuJsMH_myDxOxh8JUKY70k""}";
            var payload = "{" +
                          $@"""@type"":""WriteRequest""," +
                          $@"""iss"": ""{iss}""" +
                          $@",""aud"":""{aud}""" +
                          $@"""sub"":""{sub}""" +
                          $@"""commit"":{jwsCommit}" +
                          $@"""header"":""{header}""" +
                          $@"""@context"":""https://schema.identity.foundation/0.1""";

            var recipient = new JweRecipient(_publicKey, new Dictionary<string, object>
            {
                { "iss", keyId },
                { "sub", sub },
                { "aub", sub }
            });

            var msg = JWE.Encrypt(payload, new[] { recipient }, JweEncryption.A256GCM);
        }

        private static Dictionary<string, object> createHeader(string credentialType)
        {
            var parameters = new Dictionary<string, object>
            {
                { "context", "GAIA-X" },
                { "type", credentialType }, // e.g. RegistrationCredentials
                { "interface", "Collections" },
                { "operation", "create" }, // e.g. create
                //{ "committed_at", ZonedDateTime.now(ZoneOffset.UTC).toString() },
                { "committed_at", DateTime.Now.ToUniversalTime() }, // e.g. 2021-08-30T11:55:56.449552098Z
                { "commit_strategy", "basic" },
                { "sub", "sub" }
            };
            return parameters;
        }

        private static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}