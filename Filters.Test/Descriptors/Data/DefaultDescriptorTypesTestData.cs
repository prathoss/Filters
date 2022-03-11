using System.Collections.Generic;
using System.Threading.Tasks;
using Filters.DataDescriptor;
using Xunit;

namespace Filters.Test.Descriptors.Data;

public class DefaultDescriptorTypesTestData : TheoryData<DefaultDescriptorTypesTestRow>
{
	public DefaultDescriptorTypesTestData()
	{
		Add(
			new DefaultDescriptorTypesTestRow(
				nameof(DefaultDescriptorTypesModel.Boolean),
				new[] { "true", "false" }
			)
		);
		Add(
			new DefaultDescriptorTypesTestRow(
				nameof(DefaultDescriptorTypesModel.NullableBoolean),
				new[] { "true", "false", "null" }
			)
		);
		Add(
			new DefaultDescriptorTypesTestRow(
				nameof(DefaultDescriptorTypesModel.Enum),
				new[] { "enum1", "enum2" }
			)
		);
	}
}

public class DefaultDescriptorTypesTestRow
{
	public DefaultDescriptorTypesTestRow(string propPropName, IEnumerable<string> expectedResult)
	{
		_propName = propPropName;
		_descriptor = new DataDescriptor<DefaultDescriptorTypesModel>();
		ExpectedResult = expectedResult;
	}

	private readonly string _propName;
	private readonly DataDescriptor<DefaultDescriptorTypesModel> _descriptor;
	public readonly IEnumerable<string> ExpectedResult;

	public async Task<IEnumerable<string>> Describe()
		=> await _descriptor.Describe(_propName);

	public override string ToString()
		=> _propName;
}

public class DefaultDescriptorTypesModel
{
	public bool Boolean { get; set; }
	public bool? NullableBoolean { get; set; }
	public DefaultDescriptorEnum Enum { get; set; }
}

public enum DefaultDescriptorEnum
{
	Enum1,
	Enum2,
}