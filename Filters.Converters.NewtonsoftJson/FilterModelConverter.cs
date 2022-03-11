using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Filters.Converters.NewtonsoftJson;

public class FilterModelConverter : JsonConverter<FilterModel>
{
	private const string OperatorPropertyName = nameof(MultiFilterModel.Operator);
	private const string FilterTypePropertyName = nameof(FilterModel.FilterType);
	
	public override FilterModel ReadJson(JsonReader reader, Type objectType, FilterModel existingValue,
		bool hasExistingValue, JsonSerializer serializer)
	{
		var jObj = JObject.Load(reader);

		FilterModel model;
		// multi-filter has its conditions filter type, can not be determined by filter type
		// the presence of key operator is sufficient
		// jObj.ContainsKey dos not have StringComparison overload
		if (jObj.TryGetValue(OperatorPropertyName, StringComparison.CurrentCultureIgnoreCase, out _))
		{
			model = new MultiFilterModel();
		}
		else
		{
			var filterToken = jObj.GetValue(FilterTypePropertyName,
				StringComparison.InvariantCultureIgnoreCase);
			var filterStr = filterToken.Value<string>();
			if (!Enum.TryParse<FilterType>(filterStr, true, out var filterType))
				throw new NotSupportedException($"{FilterTypePropertyName} could not be deserialized");
			model = Converter.GetFilterModelByFilterType(filterType);
		}

		serializer.Populate(jObj.CreateReader(), model);

		return model;
	}

	public override void WriteJson(JsonWriter writer, FilterModel value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value);
	}
}