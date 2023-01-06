public class RoutingService : IRoutingService
{
    private readonly MyDbContext _context;

    public RoutingService(MyDbContext context) =>
        _context = context;

    public async Task<List<City>> FindRoutes(string startCityName, string endCityName)
    {
        var startCity = await _context.Cities.FirstOrDefaultAsync(
            city => city.Name == startCityName);

        var endCity = await _context.Cities.FirstOrDefaultAsync(
            city => city.Name == endCityName);

        if (startCity == null || endCity == null)
        {
            throw new ArgumentException("Start city or end city not found.");
        }

        return await FindShortestPathBFS(startCity, endCity);
    }

    private async Task<List<City>> FindShortestPathBFS(City startCity, City endCity)
    {
        var queue = new Queue<City>();
        var visited = new HashSet<int>();
        var predecessors = new Dictionary<int, City>();

        queue.Enqueue(startCity);
        visited.Add(startCity.Id);

        while (queue.Count > 0)
        {
            var currentCity = queue.Dequeue();

            if (currentCity.Id == endCity.Id)
            {
                return ConstructPath(startCity, endCity, predecessors);
            }

            var routes = await _context.Routes
                .Where(r => r.StartCityId == currentCity.Id || r.EndCityId == currentCity.Id)
                .Include(r => r.StartCity)
                .Include(r => r.EndCity)
                .ToListAsync();

            foreach (var route in routes)
            {
                var adjacentCity = GetAdjacentCity(currentCity, route);
                if (adjacentCity == null)
                {
                    continue;
                }

                if (!visited.Contains(adjacentCity.Id))
                {
                    queue.Enqueue(adjacentCity);
                    visited.Add(adjacentCity.Id);

                    predecessors[adjacentCity.Id] = currentCity;
                }
            }
        }
        throw new InvalidOperationException("No path found between start and end cities.");
    }

    private static List<City> ConstructPath(City startCity, City endCity, Dictionary<int, City> predecessors)
    {
        var path = new List<City>();
        var currentCity = endCity;
        while (currentCity.Id != startCity.Id)
        {
            path.Add(currentCity);
            currentCity = predecessors[currentCity.Id];
        }
        path.Add(startCity);
        path.Reverse();
        return path;
    }

    private static City? GetAdjacentCity(City currentCity, Route route)
    {
        return route.StartCityId == currentCity.Id ? route.EndCity :
        route.EndCityId == currentCity.Id ? route.StartCity : null;
    }
}
