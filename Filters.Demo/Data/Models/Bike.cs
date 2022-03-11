namespace Filters.Demo.Data.Models;

public class Bike
{
	public int Id { get; set; }
	public BikeType Type { get; set; }
	public string Manufacturer { get; set; }
	public string Model { get; set; }
}

public enum BikeType
{
	Road,
	Gravel,
	CrossCountry,
	Trail,
	Enduro,
	DownHill,
}