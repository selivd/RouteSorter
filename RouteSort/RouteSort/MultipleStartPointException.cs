using System;

namespace RouteSort
{
    internal class MultipleStartPointException : InvalidOperationException
    {
        public MultipleStartPointException(string pointName)
            : base($"Не разрешено иметь несколько маршрутов с началом в одной общей вершине [{pointName}].")
        {
        }
    }
}