var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

Configure(app);

RegisterApis(app, app.Services.GetServices<IApi>());

app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<MyDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    });

    services.AddScoped<IRoutingService, RoutingService>();
    services.AddScoped<ICityRepository, CityRepository>();
    services.AddScoped<IRouteRepository, RouteRepository>();

    services.AddTransient<IApi, CityApi>();
    services.AddTransient<IApi, RouteApi>();
}

void RegisterApis(WebApplication app, IEnumerable<IApi> apis)
{
    foreach (var api in apis)
    {
        api.Register(app);
    }
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        db.Database.EnsureCreated();
    }
    app.UseHttpsRedirection();
}