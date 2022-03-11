using System.Text.Json;
using System.Text.Json.Serialization;
using Filters.Converters.SystemTextJson;
using Filters.Test.Converters.Data;
using FluentAssertions;
using Xunit;

namespace Filters.Test.Converters;

public class SystemTextJsonConverterTest
{
	[Theory, ClassData(typeof(NewtonsoftConverterTestData))]
	public void Json_Should_ReturnModel(ConvertorTestRow row)
	{
		var filter = JsonSerializer.Deserialize<FilterModel>(
			row.Json,
			new JsonSerializerOptions (JsonSerializerDefaults.Web)
			{
				Converters =
				{
					new FilterModelConverter(),
					new JsonStringEnumConverter(),
				},
			});

		filter.Should().BeOfType(row.FilterType);
	}
}