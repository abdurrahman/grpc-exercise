using System;
using System.Collections.Generic;
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

                var request = new Point {Latitude = lat, Longitude = lon};
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
                    Lo = new Point {Latitude = lowLat, Longitude = lowLon},
                    Hi = new Point {Latitude = hiLat, Longitude = hiLon}
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

        /// <summary>
        /// Client-streaming example. Sends numPoints randomly chosen points from features 
        /// with a variable delay in between. Prints the statistics when they are sent from the server.
        /// </summary>
        public async Task RecordRoute(List<Feature> features, int numPoints)
        {
            try
            {
                Log("*** RecordRoute");
                using (var call = _client.RecordRoute())
                {
                    // Send numPoints points randomly selected from the features list.
                    var numMsg = new StringBuilder();
                    var random = new Random();
                    for (int i = 0; i < numPoints; i++)
                    {
                        var index = random.Next(features.Count);
                        var point = features[index].Location;
                        Log("Visiting point {0}, {1}", point.GetLatitude(), point.GetLongitude());

                        await call.RequestStream.WriteAsync(point);
                        
                        // A bit of delay before sending the next one.
                        await Task.Delay(random.Next(1000) + 500);
                    }

                    await call.RequestStream.CompleteAsync();

                    var summary = await call.ResponseAsync;
                    Log("Finished trip with {0} points. Passed {1} features. "
                        + "Travelled {2} meters. It took {3} seconds.", summary.PointCount,
                        summary.FeatureCount, summary.Distance, summary.ElapsedTime);
                }
                
                Log("Finished RecordRoute");
            }
            catch (RpcException ex)
            {
                Log("RPC failed", ex);
                throw;
            }   
        }

        public async Task RouteChat()
        {
            try
            {                    
                Log("*** RouteChat");
                var requests = new List<RouteNote>
                {
                    NewNote("First message", 0, 0),
                    NewNote("Second message", 0, 1),
                    NewNote("Third message", 1, 0),
                    NewNote("Fourth message", 0, 0)
                };

                using (var call = _client.RouteChat())
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Log("Got message \"{0}\" at {1}, {2}", note.Message,
                                note.Location.Latitude, note.Location.Longitude);
                        }
                    });

                    foreach (var request in requests)
                    {
                        Log("Sending message \"{0}\" at {1}, {2}", request.Message,
                            request.Location.Latitude, request.Location.Longitude);

                        await call.RequestStream.WriteAsync(request);
                    }

                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
                
                Log("Finished RouteChat");
            }
            catch (RpcException ex)
            {
                Log("RPC failed", ex);
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

        private RouteNote NewNote(string message, int lat, int lon) =>
            new RouteNote
            {
                Message = message,
                Location = new Point {Latitude = lat, Longitude = lon}
            };
    }
}