using System.Text.Json;
using System.Text.Json.Serialization;

namespace Filters.Converters.SystemTextJson;

public class FilterModelConverter : JsonConverter<FilterModel>
{
	private const string OperatorPropertyName = nameof(MultiFilterModel.Operator);
	private const string FilterTypePropertyName = nameof(FilterModel.FilterType);
	
	public override FilterModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var document = JsonDocument.ParseValue(ref reader);
		FilterModel model;

		var getPropName = (string name) => options.PropertyNamingPolicy?.ConvertName(name) ?? name;

		var operatorPropertyName = getPropName(OperatorPropertyName);
		
		if (document.RootElement.TryGetProperty(operatorPropertyName, out _))
		{
			model = new MultiFilterModel();
		}
		else
		{
			var filterTypePropertyName = getPropName(FilterTypePropertyName);
			var filterTypeJson = document.RootElement.GetProperty(filterTypePropertyName);
			if (!Enum.TryParse<FilterType>(filterTypeJson.GetString(), true, out var filterType))
				throw new NotSupportedException($"{FilterTypePropertyName} could not be deserialized");
			model = Converter.GetFilterModelByFilterType(filterType);
		}

		return (FilterModel)document.Deserialize(model.GetType(), options)!;
	}

	public override void Write(Utf8JsonWriter writer, FilterModel value, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, value, options);
}