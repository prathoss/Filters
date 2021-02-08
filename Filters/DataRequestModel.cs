using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Filters.Extensions;

namespace Filters;

public class DataRequestModel<T>
{
    public IList<SortModel> Sort { get; set; }
    public IDictionary<string, FilterModel> FilterModel { get; set; }

    //TODO: add grouping model
    public int EndRow { get; set; }
    public int StartRow { get; set; }

    public bool IsValid()
    {
        var valid = true;
        var usableColumns = typeof(T).GetProperties().Select(x => x.Name).ToList();
        if (Sort != null)
            foreach (var sort in Sort)
                if (!usableColumns.Contains(sort.ColId, StringComparer.InvariantCultureIgnoreCase))
                    valid = false;

        var param = Expression.Parameter(typeof(T));
        FilterModel?.ForEach((colId, filter) =>
        {
            var prop = Expression.Property(param, colId);
            if (!usableColumns.Contains(colId, StringComparer.InvariantCultureIgnoreCase) || !filter.IsValid(prop))
                valid = false;
        });

        //TODO: add grouping check

        return valid;
    }
}