using Filters.Converters.NewtonsoftJson;
using Filters.Test.Converters.Data;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Filters.Test.Converters;

public class NewtonsoftConverterTest
{
	[Theory, ClassData(typeof(NewtonsoftConverterTestData))]
	public void Json_Should_ReturnModel(ConvertorTestRow row)
	{
		var filter = JsonConvert.DeserializeObject<FilterModel>(row.Json, new FilterModelConverter());

		filter.Should().BeOfType(row.FilterType);
	}
}