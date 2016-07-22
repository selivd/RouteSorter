using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteSort;

namespace RouteSortTest
{
    [TestClass]
    public class ТеstNegative
    {
        [ExpectedException(typeof(CycleExistsException))]
        [TestMethod]
        public void TestSingleCycle()
        {
            RouteSorter.Sort(new[] {new Route<int>(1, 1)});
        }

        [ExpectedException(typeof(CycleExistsException))]
        [TestMethod]
        public void TestCycle()
        {
            RouteSorter.Sort(Helper.RouteBuilder<int>().Route(1,2).Route(3,4).Route(2,3).Route(4,1).Routes);
        }

        [ExpectedException(typeof(NotSingleRouteException))]
        [TestMethod]
        public void TestDuplicatedRoute()
        {
            RouteSorter.Sort(Helper.RouteBuilder<int>().Route(1, 2).Route(3, 4).Route(2, 3).Route(1,2).Routes);
        }

        [ExpectedException(typeof(NotSingleRouteException))]
        [TestMethod]
        public void TestMultipleStart()
        {
            RouteSorter.Sort(Helper.RouteBuilder<int>().Route(1, 2).Route(3, 4).Route(2, 3).Route(5, 6).Routes);
        }

        [ExpectedException(typeof(NotSingleRouteException))]
        [TestMethod]
        public void TestMultipleRoutes()
        {
            var routes = Helper.RouteBuilder<int>().Route(1, 2).Route(3, 4).Route(2, 3);
            //другой маршрут
            routes.Route(5, 6).Route(7, 8).Route(8, 5);
            RouteSorter.Sort(routes.Routes);
        }
    }
}