using RestSharp;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace execution_trade_strategy_1
{
    public class CoinbaseAuthenticator
    {
        private readonly string? _apiKey;
        private readonly string? _apiKeySecret;
        private readonly string _requestUri;
        private readonly long _timeStamp;
        private readonly Method _httpMethod;

        public CoinbaseAuthenticator()
        {
            _apiKey = Environment.GetEnvironmentVariable("COINBASE_API_KEY");
            _apiKeySecret = Environment.GetEnvironmentVariable("COINBASE_API_SECRET");
            _requestUri = "/api/v3/brokerage/orders";
            _timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _httpMethod = Method.Post;
        }

        public RestRequest GetAuthenticatedRequest(RestRequest _request, string orderPayloadJsonString)
        {
            string timeStampString = getTimeStampString();
            string preHash = getPreHash(orderPayloadJsonString);
            byte[] sha256Hash = getSHA256FromInputData(preHash);

            string signedHexidecimalStringSignature = getHexadecimalStringFromHash(sha256Hash);

            _request.AddHeader("accept", "application/json");
            _request.AddHeader("CB-ACCESS-KEY", _apiKey);
            _request.AddHeader("CB-ACCESS-SIGN", signedHexidecimalStringSignature);
            _request.AddHeader("CB-ACCESS-TIMESTAMP", timeStampString);

            return _request;
        }

        private string getHexadecimalStringFromHash(byte[] hash2)
        {
            return BitConverter.ToString(hash2).Replace("-", "").ToLower();
        }

        private byte[] getSHA256FromInputData(string preHash)
        {
            byte[] preHashBytes = Encoding.UTF8.GetBytes(preHash);
            byte[]? keyBytes = Encoding.UTF8.GetBytes(_apiKeySecret);
            HMACSHA256 hmac = new HMACSHA256(keyBytes);

            return hmac.ComputeHash(preHashBytes);
        }

        private string getPreHash(string orderPayloadJsonString)
        {
            string timestampStr = getTimeStampString();
            string httpMethodStr = _httpMethod.ToString().ToUpper();

            return timestampStr + httpMethodStr + _requestUri + orderPayloadJsonString;
        }

        private string getTimeStampString()
        {
            return _timeStamp.ToString("F0", CultureInfo.InvariantCulture);
        }
    }
}
