using System;
using Grpc.Core;
using Routeguide;

namespace GrpcClient
{
    /// <summary>
    /// Sample client code that makes gRPC calls to the server.
    /// </summary>
    public class RouteGuideClient
    {
        private readonly RouteGuide.RouteGuideClient _client;
        
        public RouteGuideClient(RouteGuide.RouteGuideClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Blocking unary call example. Calls GetFeature and prints the response.
        /// </summary>
        public void GetFeature(int lat, int lon)
        {
            try
            {
                Log("*** GetFeature: lat={0} lon={1}", lat, lon);
                
                var request = new Point { Latitude = lat, Longitude = lon };
                var feature = _client.GetFeature(request);
                if (feature.Exists())
                {
                    Log("Found feature called \"{0}\" at {1}, {2}",
                        feature.Name, feature.Location.GetLatitude(), feature.Location.GetLongitude());
                }
                else
                {
                    Log("Found no feature at {0}, {1}",
                        feature.Location.GetLatitude(), feature.Location.GetLongitude());
                }
            }
            catch (RpcException ex)
            {
                Log("RPC failed " + ex);
                throw;
            }
        }

        private void Log(string s, params object[] args)
        {
            Console.WriteLine(s, args);
        }

        private void Log(string s)
        {
            Console.WriteLine(s);
        }
    }
}