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
            // Сложность O(N) для наполнения словаря
            var connectionMap = BuildConnectionMap(routes);
            // O(N) для наполнения списка, просмотра словаря
            var result = GetSortedRoutes(connectionMap);
            // В результате сложность сортировки составила O(N)
            return result;
        }

        private static IList<IRoute<T>> GetSortedRoutes(Dictionary<T, PointDescription> connectionMap)
        {
            if (!connectionMap.Any())
                return new List<IRoute<T>>();
            // O(N) на просмотр словаря
            var startPoints = connectionMap.Where(a => a.Value.PreviousRoute == null).Take(2).ToArray();
            if (startPoints.Length == 0)
                throw new CycleExistsException();
            if (startPoints.Length > 1)
                throw new NotSingleRouteException();
            // массив не более 2 элементов, пренебрегаем, o(1) 
            var startOfPath = startPoints.Single();
            // Пренебрегая известной длиной коллекции, получаем 2*O(N) для наполнения списка из-за его перестроений.
            var result = new List<IRoute<T>>();
            var currentRoute = startOfPath.Value.NextRoute;
            // O(1) на поиск в словаре N раз
            while (currentRoute != null)
            {
                result.Add(currentRoute);
                var endPoint = currentRoute.EndPoint;
                currentRoute = connectionMap[endPoint].NextRoute;
                // это бесплатно
                connectionMap.Remove(endPoint);
            }
            // Итого O(N) - наполнение списка, просмотр словаря
            if (connectionMap.Count > 1)
                throw new NotSingleRouteException();
            return result;
        }

        private static Dictionary<T, PointDescription> BuildConnectionMap(IEnumerable<IRoute<T>> routes)
        {
            // Из-за незнания длины входной коллекции будет несколько перестроений словаря, 
            // поэтому, общее время заполнения словаря увеличится примерно в 2 раза до 2*O(N),
            // где O(N) - наполнение словаря коллекцией с известной длиной и пренебреженной вероятностью коллизий хэш кодов элементов.
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