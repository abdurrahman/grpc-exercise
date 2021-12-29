using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeter;

namespace GrpcClient
{
    class Program
    {
        private const string GrpcServerUrl = "https://localhost:5001";
        
        static async Task Main(string[] args)
        {
            // Return `true` to allow certificates that are untrusted/invalid
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using var channel = GrpcChannel.ForAddress(GrpcServerUrl, new GrpcChannelOptions{ HttpHandler = httpHandler});
            var client = new Greeter.GreeterClient(channel);

            var reply =  await client.SayHelloAsync(new HelloRequest
            {
                Name = "GrpcClient"
            });
            Console.WriteLine("Greeting: " + reply.Message);
            
            var secondReply = client.SayHelloAgain(new HelloRequest { Name = "Jennifer Lopez "});
            Console.WriteLine("Greeting: " + secondReply.Message);
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}