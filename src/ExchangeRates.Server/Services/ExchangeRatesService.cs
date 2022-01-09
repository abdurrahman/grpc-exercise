using Grpc.Core;

// Todo: Update namespace
namespace MerkezBankasi
{
    public class ExchangeRatesService : ExchangeRates.ExchangeRatesBase
    {
        private readonly ILogger _logger;

        public ExchangeRatesService(ILogger<ExchangeRatesService> logger)
        {
            _logger = logger;
        }

        public override Task<ExchangeRatesResponse> GetExchangeRates(ExchangeRatesRequest request, ServerCallContext context)
        {
            // Todo: Add exchange rates inquiry from https://www.tcmb.gov.tr/kurlar/today.xml 
            return base.GetExchangeRates(request, context);
        }
    }
}