using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public override Task<Feature> GetFeature(Point request, ServerCallContext context)
        {
            return Task.FromResult(CheckFeature(request));
        }

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