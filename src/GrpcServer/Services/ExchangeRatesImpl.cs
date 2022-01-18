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
            var exchangeRates = new List<ExchangeRate>();
            foreach (XmlNode node in exchangeRateList)
            {
                if (node.Attributes?["CurrencyCode"]?.Value == request.CurrencyCode)
                {
                    exchangeRates.Add(new ExchangeRate
                    {
                        Unit = Convert.ToInt32(node.ChildNodes[0]?.InnerText),
                        Currency = node.ChildNodes[2]?.InnerText,
                        CurrencyCode = node.Attributes?["CurrencyCode"]?.Value,
                        ForexBuying = Convert.ToDouble(node.ChildNodes[3]?.InnerText),
                        ForexSelling = Convert.ToDouble(node.ChildNodes[4]?.InnerText)
                    });
                    
                    break;
                }
            }

            response.ExchangeRate.AddRange(exchangeRates);
            return response;
        }
        
        public override async Task GetExchangeRatesStream(Empty request, IServerStreamWriter<ExchangeRate> responseStream, 
            ServerCallContext context)
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
            var xmlDoc = new XmlDocument();
            
            xmlDoc.Load(stream);

            var exchangeRateList = xmlDoc.LastChild;
            foreach (XmlNode node in exchangeRateList)
            {
                await Task.Delay(500);

                var response = new ExchangeRate
                {
                    Unit = Convert.ToInt32(node.ChildNodes[0]?.InnerText),
                    Currency = node.ChildNodes[2]?.InnerText,
                    CurrencyCode = node.Attributes?["CurrencyCode"]?.Value,
                    ForexBuying = Convert.ToDouble(node.ChildNodes[3]?.InnerText),
                    ForexSelling = Convert.ToDouble(node.ChildNodes[4]?.InnerText)
                };

                _logger.LogInformation("Sending exchange rates response");
                await responseStream.WriteAsync(response);
            }
        }
    }
}