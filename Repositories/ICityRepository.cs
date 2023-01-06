public interface ICityRepository : IDisposable
{
    Task<List<City>> GetCitiesAsync();

    Task<City> GetCityAsync(int cityId);

    Task<City?> GetCityAsync(string query);

    Task InsertCityAsync(City city);

    Task UpdateCityAsync(City city);

    Task DeleteCityAsync(int cityId);

    Task SaveAsync();
}