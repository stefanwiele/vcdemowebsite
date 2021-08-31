using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Jose;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace client_api_test_service_dotnet
{
    public class IdentityHubService
    {
        private readonly RSA _privateKey; // used to sign the jwsCommit

        private readonly RSA _publicKey; // public key of the identity hub. used to encrypt jwe object

        public IdentityHubService(string privateKey, string identityHubPublicKey)
        {
            var rsa = RSA.Create();
            _privateKey = rsa;
            _publicKey = rsa;
        }

        public void SendToIdentityHub(object commitObject)
        {
            var jwtHeaders = createHeader("TestCredentials");
            var jwtHeadersJson = JsonConvert.SerializeObject(jwtHeaders);
            var jwtHeadersBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtHeadersJson));

            var jwtPayload = commitObject;
            var jwtPayloadJson = JsonConvert.SerializeObject(jwtPayload);
            var jwtPayloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtPayloadJson));

            var jwt = CreateJws(jwtHeadersBase64, jwtPayloadBase64);

            var keyId = "kid";
            var iss = "sub";
            var aud = "aud";
            var sub = "sub";
            var test = GetJsonCommitObject(jwt, new Dictionary<string, object>
            {
                { "iss", keyId }
            });
            var payload = "{" +
                          $@"""@type"":""WriteRequest""," +
                          $@"""iss"": ""{iss}""," +
                          $@"""aud"":""{aud}""," +
                          $@"""sub"":""{sub}""," +
                          $@"""commit"":{test}," +
                          $@"""@context"":""https://schema.identity.foundation/0.1""" +
                          $@"}}";

            var recipient = new JweRecipient(JweAlgorithm.RSA_OAEP_256, _publicKey, new Dictionary<string, object>
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

        private string CreateJws(string header, string payload)
        {
            var signature = _privateKey.SignData(Convert.FromBase64String(payload),
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            return $"{header}.{payload}.{Convert.ToBase64String(signature)}";
        }

        public string GetJsonCommitObject(string serializedJwt, Dictionary<string, object> header)
        {
            var tokens = serializedJwt.Split("\\.");
            if (tokens.Length < 3)
            {
                throw new ArgumentException("Invalid jwt");
            }

            var protectedHeader = tokens[0];
            var payload = tokens[1];
            var signature = tokens[2];

            return $@"{{" +
                   $@"""protected"": ""{protectedHeader}""," +
                   $@"""payload"":""{payload}""," +
                   $@"""signature"":""{signature}""," +
                   $@"""header"":""{header}""" +
                   $@"}}";
        }
    }
}