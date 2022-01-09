using System;
using Xunit;

namespace Filters.Test;

public class ConvertorTestData : TheoryData<ConvertorTestRow>
{
	public ConvertorTestData()
	{
		Add(
			new ConvertorTestRow("Number",
			@"{
				""filterType"": ""number"",
				""type"": ""equals"",
				""filter"": ""0""
			}", 
			typeof(NumberFilterModel))
		);
		Add(
			new ConvertorTestRow("Text",
			@"{
				""filterType"": ""text"",
				""type"": ""startsWith"",
				""filter"": ""abc""
			}",
			typeof(TextFilterModel))
		);
		Add(new ConvertorTestRow("Date",
			@"{
				""filterType"": ""date"",
				""type"": ""equals"",
				""filter"": ""1970-01-01""
			}",
			typeof(DateFilterModel))
		);
		Add(new ConvertorTestRow("Set",
			@"{
				""filterType"": ""set"",
				""type"": ""equals"",
				""filter"": ""abc""
			}",
			typeof(SetFilterModel))
		);
		Add(new ConvertorTestRow("Multi",
			$@"{{
				""filterType"": ""number"",
				""operator"": ""or"",
				""condition1"": {{
					""filterType"": ""number"",
					""type"": ""equals"",
					""filter"": 1
				}},
				""condition2"": {{
					""filterType"": ""number"",
					""type"": ""equals"",
					""filter"": 2
				}}
			}}",
			typeof(MultiFilterModel)));
	}
	
	public override string ToString()
	{
		return (string)GetEnumerator().Current[0];
	}
}

public class ConvertorTestRow
{
	public ConvertorTestRow(string name, string json, Type filterType)
	{
		Name = name;
		Json = json;
		FilterType = filterType;
	}

	private string Name;
	public string Json;
	public Type FilterType;

	public override string ToString()
	{
		return Name;
	}
}