using System;
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
        readonly List<Feature> _features;
        
        public RouteGuideImpl(List<Feature> features)
        {
            _features = features;
        }

        public override Task<Feature> GetFeature(Point request, ServerCallContext context)
        {
            return Task.FromResult(CheckFeature(request));
        }

        private Feature CheckFeature(Point location)
        {
            var result = _features.FirstOrDefault((feature) => feature.Location.Equals(location));
            if (result is null)
            {
                return new Feature {Name = string.Empty, Location = location};
            }

            return result;
        }
    }
}