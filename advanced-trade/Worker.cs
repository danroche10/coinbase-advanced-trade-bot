namespace trade_execution
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                float indicatorCoinPriceChange = await getIndicatorCoinPriceChange();

                CoinbaseOrderDetails coinbaseOrderDetails = new CoinbaseOrderDetails();
                ExecuteCoinbaseTrade executeCoinbaseTrade = new ExecuteCoinbaseTrade(coinbaseOrderDetails);

                executeTrade(executeCoinbaseTrade, indicatorCoinPriceChange);

                await Task.Delay(60000, stoppingToken);
            }
        }

        private static async Task<float> getIndicatorCoinPriceChange()
        {
            List<float> indicatorCoinTodayAndYesterday = new List<float>();

            // hard code fake price from yesterday and day before
            indicatorCoinTodayAndYesterday.Add(19500);
            indicatorCoinTodayAndYesterday.Add(20000);

            string indicatorCoinId = "bitcoin";
            string indicatorCoinPriceRequestUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={indicatorCoinId}&vs_currencies=gbp";

            // remove fake price from day before yesterday
            indicatorCoinTodayAndYesterday.RemoveAt(0);

            CoinPrice coinPriceData = new CoinPrice();
            float indicatorCoinPriceToday = await coinPriceData.getIndicatorPriceToday(indicatorCoinPriceRequestUrl);
            indicatorCoinTodayAndYesterday.Add(indicatorCoinPriceToday);

            float indicatorCoinPriceChange = CoinPrice.getIndicatorCoinPriceChange(indicatorCoinTodayAndYesterday);

            return indicatorCoinPriceChange;
        }

        private static void executeTrade(ExecuteCoinbaseTrade executeCoinbaseTrade, float indicatorCoinPriceChange)
        {
            if (indicatorCoinPriceChange > 1.02)
            {
                Console.WriteLine("Buy order executed");
                //executeCoinbaseTrade.PlaceBuyOrder();
            }

            if (indicatorCoinPriceChange < 0.98)
            {
                Console.WriteLine("Sell order executed");
                //executeCoinbaseTrade.PlaceSellOrder();
            }
        }
    }
}