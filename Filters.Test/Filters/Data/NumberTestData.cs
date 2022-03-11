using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Collections;
using Xunit;

namespace Filters.Test.Filters.Data;

public class NumberTestData : TheoryData<NumberTestRow>
{
	public NumberTestData()
	{
		Add(
			new NumberTestRow("Greater than",
				new() { -2, -1, 0, 1, 2 },
				new NumberFilterModel
				{
					FilterType = FilterType.Number,
					Type = NumberFilterOptions.GreaterThan,
					Filter = 0
				},
				result => result.OnlyContain(x => x.Number > 0))
		);
		Add(
			new NumberTestRow(
				"Equals",
				new() { 0, 1, 2, 3 },
				new NumberFilterModel
				{
					FilterType = FilterType.Number,
					Type = NumberFilterOptions.Equals,
					Filter = 1
				},
				result => result.OnlyContain(x => x.Number == 1)
			)
		);
		Add(
			new NumberTestRow(
				"In range",
				new() { 0, 1, 2, 3, 4, 5, 6 },
				new NumberFilterModel
				{
					FilterType = FilterType.Number,
					Type = NumberFilterOptions.InRange,
					Filter = 2,
					FilterTo = 5
				},
				result => result.OnlyContain(x => x.Number >= 2 && x.Number <= 5)
			)
		);
	}
}

public class NumberTestRow
{
	public NumberTestRow(string name, List<int> table, FilterModel filter,
		Action<GenericCollectionAssertions<TableModel>> check)
	{
		Name = name;
		Table = table.Select(x => new TableModel(x)).ToList();
		Check = check;
		Filter = new Dictionary<string, FilterModel>
		{
			{
				nameof(TableModel.Number),
				filter
			}
		};
	}

	string Name;
	public List<TableModel> Table;
	public Action<GenericCollectionAssertions<TableModel>> Check;
	public Dictionary<string, FilterModel> Filter;

	public override string ToString()
	{
		return Name;
	}
}

public class TableModel
{
	public TableModel(int number)
	{
		Number = number;
	}

	public int Number { get; }
}