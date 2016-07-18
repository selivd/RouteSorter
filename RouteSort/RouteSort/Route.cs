using System;

namespace RouteSort
{

    /// <summary>
    /// Некоторая реализация контракта маршрута
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Route<T> : IRoute<T>
    {
        public Route(T start, T end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            StartPoint = start;
            EndPoint = end;
        }

        public T StartPoint { get; }
        
        public T EndPoint { get; }
        public override string ToString() => $"{StartPoint}->{EndPoint}";
    }
}