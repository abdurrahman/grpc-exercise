from __future__ import print_function

import logging
import asyncio
import grpc

import streamer_pb2
import streamer_pb2_grpc


def generate_messages():
    for idx in range(1, 16):
        entry_request = streamer_pb2.StreamMessage(name=f"Test {idx}", message=f"T{idx}")
        yield entry_request

def entry_request_iterator(stub):
    responses = stub.StartStreaming(generate_messages())
    for response in responses:
        print("Received message %s at %s" %
            (response.message, response.name))

def run():
    with grpc.insecure_channel('127.0.0.1:5168') as channel:
        stub = streamer_pb2_grpc.StreamServiceStub(channel)
        print("-------------- Send message --------------")
        entry_request_iterator(stub)


if __name__ == '__main__':
    logging.basicConfig()
    run()


    # while True:
    #     time.sleep(100000)