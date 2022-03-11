using Filters.Converters.NewtonsoftJson;
using Filters.Demo.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options => {
	// register FilterModelConverter to deserialize FilterModel to correct type
	options.SerializerSettings.Converters.Add(new FilterModelConverter());
	options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BikeContext>(options => {
	options.UseNpgsql(builder.Configuration.GetConnectionString("Bikes"));
});

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetService<BikeContext>();
dbContext!.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection()
	.UseAuthorization();
app.MapControllers();

app.Run();