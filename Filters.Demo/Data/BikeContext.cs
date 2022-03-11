using Filters.Demo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Filters.Demo.Data;

public class BikeContext : Microsoft.EntityFrameworkCore.DbContext
{
	public BikeContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Bike> Bikes { get; set; }

	protected override void OnModelCreating(ModelBuilder mb)
	{
		mb.Entity<Bike>().HasKey(b => b.Id);
		base.OnModelCreating(mb);

		mb.Entity<Bike>().HasData(
			new Bike {Id = 1, Type = BikeType.Trail, Manufacturer = "YT", Model = "Izzo"},
			new Bike {Id = 2,Type = BikeType.Trail, Manufacturer = "YT", Model = "Jeffsy"},
			new Bike {Id = 3,Type = BikeType.Enduro, Manufacturer = "YT", Model = "Capra"},
			new Bike {Id = 4,Type = BikeType.DownHill, Manufacturer = "YT", Model = "Tues"},
			new Bike {Id = 5,Type = BikeType.Trail, Manufacturer = "Propain", Model = "Hugene"},
			new Bike {Id = 6,Type = BikeType.Enduro, Manufacturer = "Propain", Model = "Tyee"},
			new Bike {Id = 7,Type = BikeType.Enduro, Manufacturer = "Propain", Model = "Spindrift"},
			new Bike {Id = 8,Type = BikeType.DownHill, Manufacturer = "Propain", Model = "Rage"},
			new Bike {Id = 9,Type = BikeType.Road, Manufacturer = "Canyon", Model = "Aero"},
			new Bike {Id = 10,Type = BikeType.Road, Manufacturer = "Canyon", Model = "Endurance"},
			new Bike {Id = 11,Type = BikeType.Road, Manufacturer = "Canyon", Model = "Race"},
			new Bike {Id = 12,Type = BikeType.Gravel, Manufacturer = "Canyon", Model = "Grizl"},
			new Bike {Id = 13,Type = BikeType.Gravel, Manufacturer = "Canyon", Model = "Grail"},
			new Bike {Id = 14,Type = BikeType.CrossCountry, Manufacturer = "Canyon", Model = "Exceed"},
			new Bike {Id = 15,Type = BikeType.CrossCountry, Manufacturer = "Canyon", Model = "Lux"}
		);
	}
}