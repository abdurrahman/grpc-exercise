using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Routeguide;

namespace GrpcGreeter
{
    /// <summary>
    /// Example implementation of RouteGuide server.
    /// </summary>
    public class RouteGuideImpl : RouterGuide.RouterGuideBase
    {
        private readonly List<Feature> _features;
        private readonly object _lock = new();
        private readonly Dictionary<Point, List<RouteNote>> _routeNotes = new Dictionary<Point, List<RouteNote>>();
        public RouteGuideImpl(List<Feature> features)
        {
            _features = features;
        }

        /// <summary>
        /// Gets the feature at the requested point. If no feature at that location
        /// exists, an unnammed feature is returned at the provided location.
        /// </summary>
        public override Task<Feature> GetFeature(Point request, ServerCallContext context)
        {
            return Task.FromResult(CheckFeature(request));
        }

        /// <summary>
        /// Gets all features contained within the given bounding rectangle.
        /// </summary>
        public override async Task ListFeatures(Rectangle request, IServerStreamWriter<Feature> responseStream, 
            ServerCallContext context)
        {
            var responses = _features.FindAll(feature => feature.Exists() && 
                                                         request.Contains(feature.Location));
            foreach (var response in responses)
            {
                await responseStream.WriteAsync(response);
            }
        }

        /// <summary>
        /// Gets a stream of points, and responds with statistics about the "trip": number of points,
        /// number of known features visited, total distance traveled, and total time spent.
        /// </summary>
        public override async Task<RouteSummary> RecordRoute(IAsyncStreamReader<Point> requestStream, 
            ServerCallContext context)
        {
            int pointCount = 0;
            int featureCount = 0;
            int distance = 0;
            Point previous = null;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (await requestStream.MoveNext())
            {
                var point = requestStream.Current;
                pointCount++;
                if (CheckFeature(point).Exists())
                    featureCount++;

                if (previous != null)
                {
                    distance += (int) previous.GetDistance(point);
                }

                previous = point;

            }
            stopWatch.Stop();

            return new RouteSummary
            {
                PointCount = pointCount,
                FeatureCount = featureCount,
                Distance = distance,
                ElapsedTime = (int) (stopWatch.ElapsedMilliseconds / 1000)
            };
        }

        /// <summary>
        /// Receives a stream of message/location pairs, and responds with a stream of all previous
        /// messages at each of those locations.
        /// </summary>
        public override async Task RouteChat(IAsyncStreamReader<RouteNote> requestStream, 
            IServerStreamWriter<RouteNote> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var note = requestStream.Current;
                var prevNotes = AddNoteForLocation(note.Location, note);
                foreach (var prevNote in prevNotes)
                {
                    await responseStream.WriteAsync(prevNote);
                }
            }
        }
        
        /// <summary>
        /// Adds a note for location and returns a list of pre-existing notes for that location (not containing the newly added note).
        /// </summary>
        private List<RouteNote> AddNoteForLocation(Point location, RouteNote note)
        {
            lock (_lock)
            {
                if (!_routeNotes.TryGetValue(location, out var notes))
                {   
                    _routeNotes.Add(location, notes);
                }

                var existingNotes = new List<RouteNote>(notes!);
                notes.Add(note);
                return existingNotes;
            }
        }

        /// <summary>
        /// Gets the feature at the given point.
        /// </summary>
        /// <param name="location">the location to check</param>
        /// <returns>The feature object at the point Note that an empty name indicates no feature.</returns>
        private Feature CheckFeature(Point location)
        {
            var result = _features.FirstOrDefault((feature) => feature.Location.Equals(location));
            return result ?? new Feature {Name = string.Empty, Location = location};
        }
    }
}