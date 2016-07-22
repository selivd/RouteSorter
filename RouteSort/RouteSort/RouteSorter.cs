using System.Collections.Generic;
using System.Linq;

namespace RouteSort
{
    public static class RouteSorter
    {
        /// <summary>
        ///     Метод, сортирующий маршруты в порядке их непрерывного следования, так что окончание одного становится началом
        ///     другого.
        /// </summary>
        /// <exception cref="NotSingleRouteException">Если непрерывных цепочек маршрутов более одной</exception>
        /// <exception cref="CycleExistsException">Если обнаружены циклы</exception>
        public static IList<IRoute<T>> Sort<T>(IEnumerable<IRoute<T>> routes)
        {
            return RouteSorterInternal<T>.Sort(routes);
        }
    }

    internal static class RouteSorterInternal<T>
    {
        public static IList<IRoute<T>> Sort(IEnumerable<IRoute<T>> routes)
        {
            var connectionMap = BuildConnectionMap(routes);
            var result = GetSortedRoutes(connectionMap);
            return result;
        }

        private static IList<IRoute<T>> GetSortedRoutes(Dictionary<T, PointDescription> connectionMap)
        {
            if (!connectionMap.Any())
                return new List<IRoute<T>>();
            var startPoints = connectionMap.Where(a => a.Value.PreviousRoute == null).Take(2).ToArray();
            if (startPoints.Length == 0)
                throw new CycleExistsException();
            if (startPoints.Length > 1)
                throw new NotSingleRouteException();
            var startOfPath = startPoints.Single();
            var result = new List<IRoute<T>>();
            var currentRoute = startOfPath.Value.NextRoute;
            while (currentRoute != null)
            {
                result.Add(currentRoute);
                var endPoint = currentRoute.EndPoint;
                currentRoute = connectionMap[endPoint].NextRoute;
                connectionMap.Remove(endPoint);
            }
            if (connectionMap.Count > 1)
                throw new NotSingleRouteException();
            return result;
        }

        private static Dictionary<T, PointDescription> BuildConnectionMap(IEnumerable<IRoute<T>> routes)
        {
            var connectionsMap = new Dictionary<T, PointDescription>();
            foreach (var route in routes)
            {
                PointDescription startPoint;
                if (connectionsMap.TryGetValue(route.StartPoint, out startPoint))
                {
                    if (startPoint.NextRoute != null)
                        throw new NotSingleRouteException();
                    startPoint.NextRoute = route;
                }
                else
                {
                    connectionsMap[route.StartPoint] = new PointDescription {NextRoute = route};
                }

                PointDescription endPoint;
                if (connectionsMap.TryGetValue(route.EndPoint, out endPoint))
                {
                    if (endPoint.PreviousRoute != null)
                        throw new NotSingleRouteException();
                    endPoint.PreviousRoute = route;
                }
                else
                {
                    connectionsMap[route.EndPoint] = new PointDescription {PreviousRoute = route};
                }
            }
            return connectionsMap;
        }

        private class PointDescription
        {
            public IRoute<T> NextRoute;
            public IRoute<T> PreviousRoute;
        }
    }
}