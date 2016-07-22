using System;

namespace RouteSort
{
    public class CycleExistsException : Exception
    {
        public CycleExistsException() : base("В коллекции маршрутов обнаружен замкнутный цикл")
        {
        }
    }
}