using Newtonsoft.Json.Linq;

namespace execution_trade_strategy_1
{
    public class CoinPrice
    {
        private readonly HttpClient _httpClient;

        public CoinPrice()
        {
            _httpClient = new HttpClient();
        }

        public static float getIndicatorCoinPriceChange(List<float> indicatorCoinTodayAndYesterday)
        {
            float indicatorCoinPriceChange = (indicatorCoinTodayAndYesterday.LastOrDefault() / indicatorCoinTodayAndYesterday.FirstOrDefault());

            return indicatorCoinPriceChange;
        }

        public async Task<float> getIndicatorPriceToday(string indicatorCoinPriceRequestUrl)
        {
            string responseForIndicatorCoinPriceDataContent = await getPriceDataContent(indicatorCoinPriceRequestUrl);

            float indicatorCoinPriceToday = getIndividualCoinPriceFromPriceDataContent(responseForIndicatorCoinPriceDataContent);

            return indicatorCoinPriceToday;
        }

        private float getIndividualCoinPriceFromPriceDataContent(string responseForIndicatorCoinPriceDataContent)
        {
            JObject indicatorCoinJsonObject = JObject.Parse(responseForIndicatorCoinPriceDataContent);

            Dictionary<string, Dictionary<string, float>>? indicatorCoinPriceDictionary = indicatorCoinJsonObject.ToObject<Dictionary<string, Dictionary<string, float>>>();

            return indicatorCoinPriceDictionary["bitcoin"]["gbp"];
        }

        private async Task<string> getPriceDataContent(string indicatorCoinPriceRequestUrl)
        {
            HttpResponseMessage indicatorCoinPriceDataResponse = await _httpClient.GetAsync(indicatorCoinPriceRequestUrl);

            indicatorCoinPriceDataResponse.EnsureSuccessStatusCode();

            string responseForIndicatorCoinPriceDataContent = await indicatorCoinPriceDataResponse.Content.ReadAsStringAsync();

            return responseForIndicatorCoinPriceDataContent;
        }
    }
}
