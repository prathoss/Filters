using System.Linq;
using Filters.Extensions;
using Filters.Test.Filters.Data;
using FluentAssertions;
using Xunit;

namespace Filters.Test.Filters;

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