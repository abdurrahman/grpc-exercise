using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace GrpcServer.Services
{
    public class ExchangeRatesImpl : ExchangeRates.ExchangeRatesBase
    {
        private ILogger _logger;
        
        public ExchangeRatesImpl(ILogger<ExchangeRatesImpl> logger)
        {
            _logger = logger;
        }

        public override async Task<ExchangeRatesResponse> GetExchangeRates(ExchangeRatesRequest request, 
	        ServerCallContext context)
        {
	        var httpClient = new HttpClient();
	        var stream = await httpClient.GetStreamAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
            var xmlDoc = new XmlDocument();
            
            xmlDoc.Load(stream);

            // xmlDoc.DocumentElement.SelectSingleNode("/Tarih_Date/");

            return await base.GetExchangeRates(request, context);
        }
    }
}