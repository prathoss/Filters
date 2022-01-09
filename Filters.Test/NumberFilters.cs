using System.Collections.Generic;
using System.Linq;
using Xunit;
using Filters.Extensions;
using FluentAssertions;

namespace Filters.Test;

public class NumberFilters
{
	[Theory, ClassData(typeof(NumberTestData))]
	public void Should_Filter(NumberTestRow data)
	{
		var result = data.Table.AsQueryable()
			.Filter(data.Filter)
			.ToList();

		data.Check(result.Should());
	}
}