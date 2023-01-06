public class RouteApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/routes", Get)
            .Produces<List<Route>>(StatusCodes.Status200OK)
            .WithName("GetAllRoutes")
            .WithTags("Getters");

        app.MapGet("/routes/path", Find)
            .Produces<List<City>>(StatusCodes.Status200OK)
            .WithSummary("Find the shortest path between two cities.")
            .WithName("GetShortestPath")
            .WithTags("Getters");

        app.MapGet("/routes/{id}", GetById)
            .Produces<Route>(StatusCodes.Status200OK)
            .WithName("GetRoute")
            .WithTags("Getters");

        app.MapPost("/routes", Post)
            .Accepts<Route>("application/json")
            .Produces<City>(StatusCodes.Status201Created)
            .WithDescription("Add new Routes")
            .WithName("CreateRoute")
            .WithTags("Creators");

        app.MapPut("/routes", Put)
            .Accepts<Route>("application/json")
            .WithName("UpdateRoute")
            .WithTags("Updaters");

        app.MapDelete("/routes/{id}", Delete)
            .WithName("DeleteRoute")
            .WithTags("Deleters");
    }

    private async Task<IResult> Get(IRouteRepository repository) =>
           Results.Ok(await repository.GetRoutesAsync());

    private async Task<IResult> GetById(int id, IRouteRepository repository) =>
            await repository.GetRouteAsync(id) is Route route
            ? Results.Ok(route)
            : Results.NotFound();

    private async Task<IResult> Post([FromBody] Route route, IRouteRepository repository)
    {
        await repository.InsertRouteAsync(route);
        await repository.SaveAsync();

        return Results.Created($"/route/{route.Id}", route);
    }

    private async Task<IResult> Find(
        [FromQuery][Required] string from,
        [FromQuery][Required] string to,
        IRoutingService routingService)
    {
        try
        {
            var path = await routingService.FindRoutes(from, to);
            return Results.Ok(path);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Ok(ex.Message);
        }
    }

    private async Task<IResult> Put([FromBody] Route route, IRouteRepository repository)
    {
        await repository.UpdateRouteAsync(route);
        await repository.SaveAsync();

        return Results.NoContent();
    }

    private async Task<IResult> Delete(int id, IRouteRepository repository)
    {
        await repository.DeleteRouteAsync(id);
        await repository.SaveAsync();

        return Results.NoContent();
    }
}