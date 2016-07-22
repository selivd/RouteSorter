using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteSort;

namespace RouteSortTest
{
    [TestClass]
    public class ТеstPositive
    {
        [TestMethod]
        public void TestEmpty()
        {
            var result = RouteSorter.Sort(new List<IRoute<int>>());
            Assert.IsNotNull(result, "Пустой маршрут не должен быть null");
            Assert.AreEqual(0, result.Count, "Маршрут должен быть пуст");
        }

        [TestMethod]
        public void TestSingleRouteInt()
        {
            var routes =
                Helper.RouteBuilder<int>()
                    .Route(1, 2)
                    .Routes;

            CheckSort(RouteSorter.Sort(routes), 1, 2);
        }

        [TestMethod]
        public void TestInt()
        {
            var routes =
                Helper.RouteBuilder<int>()
                    .Route(3, 4)
                    .Route(10, 20)
                    .Route(4, 6)
                    .Route(20, 3)
                    .Routes;
            CheckSort(RouteSorter.Sort(routes), 10, 20, 3, 4, 6);
        }

        [TestMethod]
        public void TestSingleRouteString()
        {
            var routes =
                Helper.RouteBuilder<string>()
                    .Route("a", "b")
                    .Routes;
            CheckSort(RouteSorter.Sort(routes), "a", "b");
        }

        [TestMethod]
        public void TestString()
        {
            var routes =
                Helper.RouteBuilder<string>()
                    .Route("3", "a")
                    .Route("10", "20")
                    .Route("a", "b")
                    .Route("20", "3")
                    .Routes;
            CheckSort(RouteSorter.Sort(routes), "10", "20", "3", "a", "b");
        }

        private static void CheckSort<T>(IList<IRoute<T>> sortedList, params T[] expectedPath)
        {
            var resultNodes = new List<T>();
            if (sortedList.Any())
                resultNodes.Add(sortedList.First().StartPoint);
            resultNodes.AddRange(sortedList.Select(sortedRoute => sortedRoute.EndPoint));

            var expectedLog = string.Join("->", expectedPath);
            var resultLog = string.Join("->", resultNodes);

            Assert.AreEqual(expectedPath.Count(), resultNodes.Count,
                $"Длина общего пути не совпадает: \r\nexpected {expectedLog}\r\nactual {resultLog}");
            var toCompare = resultNodes.Zip(expectedPath,
                (resultNode, expected) => new {ResultNode = resultNode, Expected = expected});

            foreach (var pair in toCompare)
            {
                Assert.AreEqual(pair.Expected, pair.ResultNode,
                    $"Общий путь неверно построен: \r\nexpected {expectedLog}\r\nactual {resultLog}");
            }
        }
    }
}