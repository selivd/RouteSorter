using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteSort;

namespace RouteSortTest
{
    internal static class Helper
    {
        public class SugarBulder<T>
        {
            readonly List<IRoute<T>> list = new List<IRoute<T>>(); 

            public SugarBulder<T> Route(T start, T end)
            {
                list.Add(new Route<T>(start, end));
                return this;
            }

            public IList<IRoute<T>> Routes => list.AsReadOnly();

            public void Test(Action<IList<IRoute<T>>> assert)
            {
                assert(list);
            }
        }

        public static SugarBulder<T> RouteBuilder<T>()
        {
            return new SugarBulder<T>();
        }

        public static void TestPositive<T>(IList<IRoute<T>> routes)
        {
            var sortedRoutes = RouteSorter.Sort(routes);
            IRoute<T> prev = null;
            foreach (var sortedRoute in sortedRoutes)
            {
                if (prev == null)
                {
                    prev = sortedRoute;
                    continue;
                }
                Assert.AreEqual(prev.EndPoint, sortedRoute.StartPoint, "Маршруты не состыкованы");
            }
        }
        //public static void TestPositive<T>(params T[] vals)
        //{
        //    var starts = vals.Where((v, i) => i%2 == 0);
        //    var ends = vals.Where((v, i) => i%2 == 1);
        //    var routes = starts.Zip(ends, (s, e) => new Route<T>(s, e)).ToList();
        //    var sortedRoutes = RouteSorter.Sort(routes);
        //    IRoute<T> prev = null;
        //    foreach (var sortedRoute in sortedRoutes)
        //    {
        //        if (prev == null)
        //        {
        //            prev = sortedRoute;
        //            continue;
        //        }
        //        Assert.AreEqual(prev.EndPoint, sortedRoute.StartPoint, "Маршруты не состыкованы");
        //    }
        //}
    }

}