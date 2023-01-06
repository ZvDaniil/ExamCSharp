public class Route
{
    public int Id { get; set; }
    public int StartCityId { get; set; }
    public City StartCity { get; set; } = default!;
    public int EndCityId { get; set; }
    public City EndCity { get; set; } = default!;
}