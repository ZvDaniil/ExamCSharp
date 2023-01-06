public class CityRepository : ICityRepository
{
    private readonly MyDbContext _context;

    public CityRepository(MyDbContext context) =>
        _context = context;

    public async Task<List<City>> GetCitiesAsync() =>
        await _context.Cities.ToListAsync();

    public async Task<City> GetCityAsync(int cityId) =>
        await _context.Cities.FirstAsync(city => city.Id.Equals(cityId));

    public async Task<City?> GetCityAsync(string query) =>
       await _context.Cities.FirstOrDefaultAsync(c => c.Name.Contains(query));

    public async Task InsertCityAsync(City city) =>
        await _context.Cities.AddAsync(city);

    public async Task UpdateCityAsync(City city)
    {
        var cityFromDb = await _context.Cities.FindAsync(
            new object[] { city.Id });

        if (cityFromDb != null)
        {
            cityFromDb.Name = city.Name;
        }
    }

    public async Task DeleteCityAsync(int cityId)
    {
        var cityFromDb = await _context.Cities.FindAsync(
           new object[] { cityId });

        if (cityFromDb != null)
        {
            _context.Cities.Remove(cityFromDb);
        }
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if ((!_disposed) && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}