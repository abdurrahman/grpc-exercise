using System.Collections.Generic;
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