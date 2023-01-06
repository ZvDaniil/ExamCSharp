public class RouteRepository : IRouteRepository
{
    private readonly MyDbContext _context;

    public RouteRepository(MyDbContext context) =>
        _context = context;

    public async Task<List<Route>> GetRoutesAsync() =>
        await _context.Routes
            .Include(route => route.StartCity)
            .Include(route => route.EndCity)
            .ToListAsync();

    public async Task<Route> GetRouteAsync(int routeId) =>
        await _context.Routes
            .Include(route => route.StartCity)
            .Include(route => route.EndCity)
            .FirstAsync(route => route.Id == routeId);

    public async Task InsertRouteAsync(Route route) =>
        await _context.Routes.AddAsync(route);

    public async Task UpdateRouteAsync(Route route)
    {
        var routeFromDb = await _context.Routes.FindAsync(
            new object[] { route.Id });

        if (routeFromDb != null)
        {
            routeFromDb.StartCityId = route.StartCityId;
            routeFromDb.EndCityId = route.EndCityId;
        }
    }

    public async Task DeleteRouteAsync(int routeId)
    {
        var routeFromDb = await _context.Routes.FindAsync(
            new object[] { routeId });

        if (routeFromDb != null)
        {
            _context.Routes.Remove(routeFromDb);
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