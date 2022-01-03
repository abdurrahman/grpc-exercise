using System;
using System.Text;
using System.Threading.Tasks;
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
        
        /// <summary>
        /// Server-streaming example. Calls listFeatures with a rectangle of interest. Prints each response feature as it arrives.
        /// </summary>
        public async Task ListFeatures(int lowLat, int lowLon, int hiLat, int hiLon)
        {
            try
            {
                Log("*** ListFeatures: lowLat={0} lowLon={1} hiLat={2} hiLon={3}", lowLat, lowLon, hiLat,
                    hiLon);
                var request = new Rectangle
                {
                    Lo = new Point { Latitude = lowLat, Longitude = lowLon },
                    Hi = new Point { Latitude = hiLat, Longitude = hiLon }
                };

                using var call = _client.ListFeatures(request);
                var responseStream = call.ResponseStream;
                var responseLog = new StringBuilder("Result: ");

                while (await responseStream.MoveNext())
                {
                    var feature = responseStream.Current;
                    responseLog.Append(feature);
                }
                Log(responseLog.ToString());
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