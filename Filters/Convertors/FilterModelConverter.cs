using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Filters.Convertors;

public class FilterModelConverter : JsonConverter<FilterModel>
{
    public override FilterModel ReadJson(JsonReader reader, Type objectType, FilterModel existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var jObj = JObject.Load(reader);

        FilterModel model;
        // multi-filter has its conditions filter type, can not be determined by filter type
        // the presence of key operator is sufficient
        // jObj.ContainsKey dos not have StringComparison overload
        if (jObj.TryGetValue(nameof(MultiFilterModel.Operator), StringComparison.CurrentCultureIgnoreCase, out _))
        {
            model = new MultiFilterModel();
        }
        else
        {
            var filterToken = jObj.GetValue(nameof(FilterModel.FilterType),
                StringComparison.InvariantCultureIgnoreCase);
            var filterStr = filterToken.Value<string>();
            if (!Enum.TryParse<FilterType>(filterStr, true, out var filterType))
                throw new NotSupportedException($"{nameof(FilterModel.FilterType)} could not be deserialized");
            model = filterType switch
            {
                FilterType.Text => new TextFilterModel(),
                FilterType.Number => new NumberFilterModel(),
                FilterType.Date => new DateFilterModel(),
                FilterType.Set => new SetFilterModel(),
                _ => throw new NotImplementedException(),
            };
        }

        serializer.Populate(jObj.CreateReader(), model);

        return model;
    }

    public override void WriteJson(JsonWriter writer, FilterModel value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}