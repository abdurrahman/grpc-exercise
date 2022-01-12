using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Exchangerates //GrpcServer.Services
{
    public class ExchangeRatesImpl : Exchangerates.ExchangeRatesBase
    {
        private ILogger _logger;
        
        public ExchangeRatesImpl(ILogger<ExchangeRatesImpl> logger)
        {
            _logger = logger;
        }
    }
}