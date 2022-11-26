using System;
using System.Globalization;

namespace Jha.Services.TweetsCollectorService.Utils.Authentication.OAuth
{
    public class Authenticator
    {
        #region Private members

        private const string CustormerKeyParamName = "oauth_consumer_key";
        private const string NonceParamName = "oauth_nonce";
        private const string SignatureParamName = "oauth_signature";
        private const string SignatureMethodParamName = "oauth_signature_method";
        private const string TokenParamName = "oauth_token";
        private const string VersionParamName = "oauth_version";

        private const string SignatureMethod = "HMAC-SHA1";
        private const string AuthVersion = "";


        private readonly string apiKey;
        private readonly string apiSecret;

        #endregion

        public Authenticator(string apiKey, string apiSecret)
        {
            this.apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            this.apiSecret = apiSecret ?? throw new ArgumentNullException(nameof(apiSecret));
        }

        public string CreateAuthorizationHeader()
        {
            var parameters = new SortedDictionary<string, string>
                {
                    { CustormerKeyParamName, this.apiKey },
                    { NonceParamName, Guid.NewGuid().ToString("N") },
                    { SignatureParamName, "" },
                    { SignatureMethodParamName, SignatureMethod },
                    { TokenParamName, SignatureMethod },
                };

            throw new NotImplementedException();
        }

        private static string Timestamp()
        {
            long timestamp = (long)((DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds);
            return timestamp.ToString(CultureInfo.InvariantCulture);
        }
    }
}

