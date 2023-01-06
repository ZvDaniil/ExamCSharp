public class CityApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/cities", Get)
            .Produces<List<City>>(StatusCodes.Status200OK)
            .WithName("GetAllCities")
            .WithTags("Getters");

        app.MapGet("/cities/{id}", GetById)
            .Produces<City>(StatusCodes.Status200OK)
            .WithName("GetCity")
            .WithTags("Getters");

        app.MapPost("/cities", Post)
            .Accepts<City>("application/json")
            .Produces<City>(StatusCodes.Status201Created)
            .WithName("CreatorCity")
            .WithTags("Creators");

        app.MapPut("/cities", Put)
            .Accepts<City>("application/json")
            .WithName("UpdateCity")
            .WithTags("Updaters");

        app.MapDelete("/cities/{id}", Delete)
            .WithName("DeleteCity")
            .WithTags("Deleters");
    }

    private async Task<IResult> Get(ICityRepository repository) =>
           Results.Ok(await repository.GetCitiesAsync());

    private async Task<IResult> GetById(int id, ICityRepository repository) =>
            await repository.GetCityAsync(id) is City city
            ? Results.Ok(city)
            : Results.NotFound();

    private async Task<IResult> Post([FromBody] City city, ICityRepository repository)
    {
        await repository.InsertCityAsync(city);
        await repository.SaveAsync();

        return Results.Created($"/city/{city.Id}", city);
    }

    private async Task<IResult> Put([FromBody] City city, ICityRepository repository)
    {
        await repository.UpdateCityAsync(city);
        await repository.SaveAsync();

        return Results.NoContent();
    }

    private async Task<IResult> Delete(int id, ICityRepository repository)
    {
        await repository.DeleteCityAsync(id);
        await repository.SaveAsync();

        return Results.NoContent();
    }
}