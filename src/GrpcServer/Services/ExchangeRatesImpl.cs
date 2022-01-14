using System;
using System.Collections.Generic;
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

            var response = new ExchangeRatesResponse();
            var exchangeRateList = xmlDoc.LastChild; // Tarih_Date
            foreach (XmlNode node in exchangeRateList)
            {
                if (node.Attributes["CurrencyCode"].Value == request.CurrencyCode)
                {
                    response.Unit = Convert.ToInt32(node.ChildNodes[0]?.InnerText);
                    response.Currency = node.ChildNodes[2]?.InnerText;
                    response.CurrencyCode = node.Attributes["CurrencyCode"]?.Value;
                    response.ForexBuying = Convert.ToDouble(node.ChildNodes[3]?.InnerText);
                    response.ForexSelling = Convert.ToDouble(node.ChildNodes[4]?.InnerText);
                }
            }

            return response;
        }
    }
}