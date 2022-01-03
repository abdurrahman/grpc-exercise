using System;
using System.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using Routeguide;

namespace GrpcClient
{
    class Program
    {
        private const string GrpcServerUrl = "https://localhost:5001";
        
        static void Main(string[] args)
        {           
            // Return `true` to allow certificates that are untrusted/invalid
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            #region Greetings
            
            // using var channel = GrpcChannel.ForAddress(GrpcServerUrl, new GrpcChannelOptions{ HttpHandler = httpHandler});
            // var client = new Greeter.GreeterClient(channel);
            //
            // var reply =  await client.SayHelloAsync(new HelloRequest
            // {
            //     Name = "GrpcClient"
            // });
            // Console.WriteLine("Greeting: " + reply.Message);
            //
            // var secondReply = client.SayHelloAgain(new HelloRequest { Name = "Jennifer Lopez "});
            // Console.WriteLine("Greeting: " + secondReply.Message);
            //
            // Console.WriteLine("Press any key to exit...");
            // Console.ReadKey();

            #endregion

            // using var channel = GrpcChannel.ForAddress(GrpcServerUrl, new GrpcChannelOptions{ HttpHandler = httpHandler});
            var channel = new Channel("127.0.0.1:30052", ChannelCredentials.Insecure);
            var client = new RouteGuideClient(new RouteGuide.RouteGuideClient(channel));
            
            // Looking for a valid feature
            client.GetFeature(409146138, -746188906);
            // Feature missing.
            client.GetFeature(0, 0);
            
            // Looking for features between 40, -75 and 42, -73.
            client.ListFeatures(400000000, -750000000, 420000000, -730000000).Wait();

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}