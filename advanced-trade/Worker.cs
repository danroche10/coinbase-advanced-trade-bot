namespace trade_execution
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private int _executionCount = 0;
        private List<float> _indicatorCoinTodayAndYesterday = new List<float>();
        private readonly int oneHourInMilliseconds = 3600000;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _executionCount++;

                Console.WriteLine($"Execution Count: {_executionCount}");

                await updatePriceComparisonList();
                
                if (_executionCount < 3)
                {
                    await Task.Delay(oneHourInMilliseconds, stoppingToken);
                    continue;
                }

                float indicatorCoinPriceChange = getIndicatorCoinPriceChange();
                CoinbaseOrderDetails coinbaseOrderDetails = new CoinbaseOrderDetails();
                ExecuteCoinbaseTrade executeCoinbaseTrade = new ExecuteCoinbaseTrade(coinbaseOrderDetails);

                executeTrade(executeCoinbaseTrade, indicatorCoinPriceChange);

                await Task.Delay(oneHourInMilliseconds, stoppingToken);
            }
        }

        private async Task updatePriceComparisonList()
        {
            string indicatorCoinId = "bitcoin";
            string indicatorCoinPriceRequestUrl = $"https://api.coingecko.com/api/v3/simple/price?ids={indicatorCoinId}&vs_currencies=gbp";

            if (_executionCount > 2)
            {
                _indicatorCoinTodayAndYesterday.RemoveAt(0);
            }
            
            CoinPrice coinPriceData = new CoinPrice();
            float indicatorCoinPriceToday = await coinPriceData.getIndicatorPriceToday(indicatorCoinPriceRequestUrl);
            _indicatorCoinTodayAndYesterday.Add(indicatorCoinPriceToday);
            Console.WriteLine($"indicatorCoinPriceToday: {_indicatorCoinTodayAndYesterday.FirstOrDefault()}, indicatorCoinPriceToday: {_indicatorCoinTodayAndYesterday.LastOrDefault()}");
        }

        private float getIndicatorCoinPriceChange()
        {
            float indicatorCoinPriceChange = CoinPrice.getIndicatorCoinPriceChange(_indicatorCoinTodayAndYesterday);
            Console.WriteLine($"indicator price change: {indicatorCoinPriceChange}");
            return indicatorCoinPriceChange;
        }

        private static void executeTrade(ExecuteCoinbaseTrade executeCoinbaseTrade, float indicatorCoinPriceChange)
        {
            if (indicatorCoinPriceChange > 1.005)
            {
                Console.WriteLine("Buy order executed");
                executeCoinbaseTrade.PlaceBuyOrder();
            }

            if (indicatorCoinPriceChange < 0.995)
            {
                Console.WriteLine("Sell order executed");
                executeCoinbaseTrade.PlaceSellOrder();
            }
        }
    }
}