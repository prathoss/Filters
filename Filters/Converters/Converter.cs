using System;

namespace Filters.Converters;

public static class Converter
{
	public static FilterModel GetFilterModelByFilterType(FilterType filterType)
	=> filterType switch
	{
		FilterType.Text => new TextFilterModel(),
		FilterType.Number => new NumberFilterModel(),
		FilterType.Date => new DateFilterModel(),
		FilterType.Set => new SetFilterModel(),
		_ => throw new NotSupportedException(),
	};
}