using Newtonsoft.Json.Linq;
namespace trade_execution
{
    public class CoinbaseOrderDetails
    {
        public string GetBuyOrderDetails()
        {
            string orderPayloadString = File.ReadAllText("eth_buy_order_data.json");

            JObject orderJsonObject = JObject.Parse(orderPayloadString);

            orderJsonObject.Add("client_order_id", Guid.NewGuid().ToString());

            return orderJsonObject.ToString();
        }

        public string GetSellOrderDetails()
        {
            string orderPayloadString = File.ReadAllText("eth_sell_order_data.json");

            JObject orderJsonObject = JObject.Parse(orderPayloadString);

            orderJsonObject.Add("client_order_id", Guid.NewGuid().ToString());

            return orderJsonObject.ToString();
        }
    }
}
