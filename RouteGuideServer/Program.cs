using System;
using Grpc.Core;

namespace Routeguide
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 30052;

            var features = RouteGuideUtil.LoadFeatures();

            Server server = new Server
            {
                Services = { RouteGuide.BindService(new RouteGuideImpl(features)) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure )}
            };
            server.Start();
            
            Console.WriteLine("RouteGuide server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}