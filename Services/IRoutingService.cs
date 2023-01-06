public interface IRoutingService
{
    Task<List<City>> FindRoutes(string startCityName, string endCityName);
}