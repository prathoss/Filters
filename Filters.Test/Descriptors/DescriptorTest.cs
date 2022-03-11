using System.Threading.Tasks;
using Filters.Test.Descriptors.Data;
using FluentAssertions;
using Xunit;

namespace Filters.Test.Descriptors;

public class DescriptorTest
{
	[Theory, ClassData(typeof(DefaultDescriptorTypesTestData))]
	public async Task Default_Descriptions(DefaultDescriptorTypesTestRow data)
	{
		var result = await data.Describe();
		result.Should().BeEquivalentTo(data.ExpectedResult);
	}
}