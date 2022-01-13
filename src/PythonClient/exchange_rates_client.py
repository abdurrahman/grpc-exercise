from __future__ import print_function

import logging
import grpc
import exchange_rates_pb2
import exchange_rates_pb2_grpc

def get_exchange_rates(stub):
    result = stub.GetExchangeRates(exchange_rates_pb2.ExchangeRatesRequest(currencyCode = "TRY"))
    print("GetExchangeRates called %s" % (result))

def run():
    with grpc.insecure_channel('127.0.0.1:5168') as channel:
        stub = exchange_rates_pb2_grpc.ExchangeRatesStub(channel)
        
        print("-------------- Get Exchange Rates --------------")
        get_exchange_rates(stub)
    
if __name__ == '__main__':
    logging.basicConfig()
    run()