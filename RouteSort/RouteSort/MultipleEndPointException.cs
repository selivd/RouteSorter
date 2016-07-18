using System;

namespace RouteSort
{
    public class MultipleEndPointException : InvalidOperationException
    {
        public MultipleEndPointException(string pointName)
            : base($"Не разрешено иметь несколько маршрутов с окончанием в одной общей вершине [{pointName}].")
        {
        }
    }
}