from __future__ import print_function

import logging
import asyncio
import grpc
import exchange_rates_pb2
import exchange_rates_pb2_grpc

def get_exchange_rates(stub):
    result = stub.GetExchangeRates(exchange_rates_pb2.ExchangeRatesRequest(currencyCode = "USD"))
    print("GetExchangeRates called %s" % (result))

def get_exchange_rates_stream(stub):
     for s in stub.GetExchangeRatesStream(exchange_rates_pb2.Empty()):
         print("GetExchangeRates called %s" % (s))
         print("---------")

def run():
    with grpc.insecure_channel('127.0.0.1:5168') as channel:
        stub = exchange_rates_pb2_grpc.ExchangeRatesStub(channel)
        
        print("-------------- Get Exchange Rates --------------")
        get_exchange_rates(stub)
        print("-------------- Get Exchange Rates Stream --------------")
        get_exchange_rates_stream(stub)
    
if __name__ == '__main__':
    logging.basicConfig()
    run()

    # while True:
    #     time.sleep(100000)