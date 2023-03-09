using RestSharp;

namespace trade_execution
{
    public class ExecuteCoinbaseTrade
    {
        private CoinbaseAuthenticator _authenticator;
        private CoinbaseOrderDetails _orderDetails;
        private readonly string _fullRequestUri = "https://api.coinbase.com/api/v3/brokerage/orders";
        public ExecuteCoinbaseTrade(CoinbaseOrderDetails coinbaseOrderDetails)
        {
            _authenticator = new CoinbaseAuthenticator();
            _orderDetails = coinbaseOrderDetails;
        }

        public void PlaceSellOrder()
        {
            string coinbaseOrderJsonString = _orderDetails.GetSellOrderDetails();

            RestRequest authenticatedOrderRequest = getAuthenticatedOrderRequest(coinbaseOrderJsonString);

            authenticatedOrderRequest.AddBody(coinbaseOrderJsonString);

            ExecuteTrade(authenticatedOrderRequest);
        }

        public void PlaceBuyOrder()
        {
            string coinbaseOrderJsonString = _orderDetails.GetBuyOrderDetails();

            RestRequest authenticatedOrderRequest = getAuthenticatedOrderRequest(coinbaseOrderJsonString);

            authenticatedOrderRequest.AddBody(coinbaseOrderJsonString);

            ExecuteTrade(authenticatedOrderRequest);
        }

        private RestRequest getAuthenticatedOrderRequest(string coinbaseOrderJsonString)
        {
            RestRequest createOrderRequest = new RestRequest(_fullRequestUri, Method.Post);

            return _authenticator.GetAuthenticatedRequest(createOrderRequest, coinbaseOrderJsonString);
        }

        private void ExecuteTrade(RestRequest authenticatedOrderRequest)
        {
            RestClient client = new RestClient(_fullRequestUri);

            RestResponse response = client.Execute(authenticatedOrderRequest);

            Console.WriteLine(response.Content);
        }
    }
}