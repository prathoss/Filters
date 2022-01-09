using System;
using System.Collections.Generic;
using Filters.Convertors;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Filters.Test;

public class ConvertorTest
{
	[Theory, ClassData(typeof(ConvertorTestData))]
	public void Json_Should_ReturnModel(ConvertorTestRow row)
	{
		var filter = JsonConvert.DeserializeObject<FilterModel>(row.Json, new FilterModelConverter());

		filter.Should().BeOfType(row.FilterType);
	}
}