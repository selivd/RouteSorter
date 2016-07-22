using System;

namespace RouteSort
{
    public class NotSingleRouteException : Exception
    {
        public NotSingleRouteException()
            : base("Коллекция маршрутов образует несколько общих маршрутных цепочек")
        {
        }
    }
}