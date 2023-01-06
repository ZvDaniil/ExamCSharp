public class MyDbContext : DbContext
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Route> Routes => Set<Route>();
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
}