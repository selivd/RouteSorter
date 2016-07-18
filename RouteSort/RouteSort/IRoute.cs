namespace RouteSort
{
    /// <summary>
    /// Информация о маршруте.
    /// </summary>
    public interface IRoute<T>
    {
        /// <summary>
        ///     Начало маршрута
        /// </summary>
        T StartPoint { get; }

        /// <summary>
        ///     Окончание маршрута
        /// </summary>
        T EndPoint { get; }
    }
}