using System;
using System.Linq;

namespace RouteSort
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Demo("Поиск связанного маршрута", "1", "2", "3", "4", "2", "3");
            Demo("Обработка ошибок", "1", "1"); //CycleExistException
        }

        private static void Demo<T>(string title, params T[] vals)
        {
            try
            {
                Console.WriteLine($"========{title}========");
                var starts = vals.Where((v, i) => i%2 == 0);
                var ends = vals.Where((v, i) => i%2 == 1);
                var routes = starts.Zip(ends, (s, e) => new Route<T>(s, e)).ToList();
                var inputRoutesLog = string.Join(",", routes);
                Console.WriteLine("Маршруты на входе");
                Console.WriteLine(inputRoutesLog);
                var sortedRoutes = RouteSorter.Sort(routes);
                Console.WriteLine("Отсортированные маршруты");
                var outputLog = string.Join("\r\n", sortedRoutes);
                Console.WriteLine(outputLog);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}