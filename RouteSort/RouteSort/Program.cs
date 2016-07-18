using System;

namespace RouteSort.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var routes = new[] {new Route<int>(1, 2), new Route<int>(3, 4), new Route<int>(2, 3)};
            var sortedRoutes = RouteSorter.Sort(routes);
            var log = string.Join("\r\n", sortedRoutes);
            Console.WriteLine(log);
        }
    }
}