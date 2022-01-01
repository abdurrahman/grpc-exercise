using System;
using Routeguide;

namespace GrpcGreeter
{
    /// <summary>
    /// Utility methods for the route guide example.
    /// </summary>
    public static class RouteGuideUtil
    {
        private const string DefaultFeaturesResourceName = "route_guide_db.json";
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns <c>true</c> if rectangular area contains given point.
        /// </summary>
        public static bool Contains(this Rectangle rectangle, Point point)
        {
            throw new NotImplementedException();
        }
    }
}