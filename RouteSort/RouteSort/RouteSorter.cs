using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteSort
{

    public static class RouteSorter
    {
        /// <summary>
        /// Метод, сортирующий маршруты в порядке их непрерывного следования, так что окончание одного становится началом другого.
        /// </summary>
        /// <exception cref="InvalidOperationException">Если непрерывных цепочек маршрутов более одной</exception>
        /// <exception cref="MultipleStartPointException">Если несколько маршрутов начинаются некоторым пунктом</exception>
        /// <exception cref="MultipleEndPointException">Если несколько маршрутов оканчиваются некоторым пунктом</exception>
        public static IList<IRoute<T>> Sort<T>(IEnumerable<IRoute<T>> routes)
        {
            return RouteSorterInternal<T>.Sort(routes);
        }
    }
    static class RouteSorterInternal<T>
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

            var startOfPath = connectionMap.Single(a => a.Value.PreviousRoute == null);
            var result = new List<IRoute<T>>();
            var currentRoute = startOfPath.Value.NextRoute;
            while (currentRoute != null)
            {
                result.Add(currentRoute);
                currentRoute = connectionMap[currentRoute.EndPoint].NextRoute;
            }
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
                        throw new MultipleStartPointException($"{route.StartPoint}");
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
                        throw new MultipleEndPointException($"{route.EndPoint}");
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