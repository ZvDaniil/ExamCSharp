public interface IRouteRepository : IDisposable
{
    Task<List<Route>> GetRoutesAsync();

    Task<Route> GetRouteAsync(int cityId);

    Task InsertRouteAsync(Route city);

    Task UpdateRouteAsync(Route city);

    Task DeleteRouteAsync(int cityId);

    Task SaveAsync();
}