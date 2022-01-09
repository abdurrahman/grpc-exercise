using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Routeguide
{
    /// <summary>
    /// Utility methods for the route guide example.
    /// </summary>
    public static class RouteGuideUtil
    {
        private const string DefaultFeaturesResourceName = "RouteGuide.route_guide_db.json";
        private const double CoordFactor = 1e7;

        public static bool Exists(this Feature feature)
        {
            return feature != null && feature.Name.Length != 0;
        }

        public static double GetLatitude(this Point point)
            => point.Latitude / CoordFactor;

        public static double GetLongitude(this Point point)
            => point.Longitude / CoordFactor;

        /// <summary>
        /// Calculate the distance between two points using the "haversine" formula.
        /// The formula is based on http://mathforum.org/library/drmath/view/51879.html
        /// </summary>
        /// <param name="start">the starting point</param>
        /// <param name="end">the end point</param>
        /// <returns>the distance between the points in meters</returns>
        public static double GetDistance(this Point start, Point end)
        {
            const int radius = 6371000; // earth radius in metres
            var startLatitude = ToRadians(start.GetLatitude());
            var endLatitude = ToRadians(end.GetLatitude());
            var startLongitude = ToRadians(start.GetLongitude());
            var endLongitude = ToRadians(end.GetLongitude());

            var deltaLatitude = endLatitude - startLatitude;
            var deltaLongitude = endLongitude - startLongitude;

            double a = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) + Math.Cos(startLatitude) *
                Math.Cos(endLatitude) * Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return radius * c;
        }

        /// <summary>
        /// Returns <c>true</c> if rectangular area contains given point.
        /// </summary>
        public static bool Contains(this Rectangle rectangle, Point point)
        {
            var left = Math.Min(rectangle.Lo.Longitude, rectangle.Hi.Longitude);
            var right = Math.Max(rectangle.Lo.Longitude, rectangle.Hi.Longitude);
            var top = Math.Max(rectangle.Lo.Latitude, rectangle.Hi.Latitude);
            var bottom = Math.Min(rectangle.Lo.Latitude, rectangle.Hi.Latitude);

            return point.Longitude >= left && point.Longitude <= right && point.Latitude >= bottom &&
                   point.Latitude <= top;
        }

        private static double ToRadians(double val)
            => (Math.PI / 180) * val;

        public static List<Feature> LoadFeatures()
        {
            var features = new List<Feature>();
            var jsonFeatures = JsonConvert.DeserializeObject<List<JsonFeature>>(ReadFeaturesFromResources());

            foreach (var jsonFeature in jsonFeatures)
            {
                features.Add(new Feature
                {
                    Name = jsonFeature.name,
                    Location = new Point 
                    { 
                        Longitude = jsonFeature.location.longitude, 
                        Latitude = jsonFeature.location.latitude
                    }
                });
            }
            return features;
        }

        private static string ReadFeaturesFromResources()
        {
            var stream = typeof(RouteGuideUtil).GetTypeInfo().Assembly
                .GetManifestResourceStream(DefaultFeaturesResourceName);
            if (stream is null)
            {
                throw new IOException(string.Format("Error loading the embedded resource \"{0}\""));
            }
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

#pragma warning disable 0649 // Suppresses "Field 'x' is never assigned to".
        private class JsonFeature
        {
            public string name;
            public JsonLocation location;
        }

        private class JsonLocation
        {
            public int longitude;
            public int latitude;
        }
#pragma warning restore 0649
    }
}