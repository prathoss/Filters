using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Filters.Extensions;

public static class IQueryableExtensions
{
    /// <summary>
    ///     Creates dynamic linq ordering expressions from model
    /// </summary>
    public static IQueryable<T> Sort<T>(this IQueryable<T> q, IList<SortModel> sorts)
    {
        if (sorts == null || !sorts.Any()) return q;

        var param = Expression.Parameter(q.ElementType);

        var first = sorts[0];
        var sortMethodName = first.Sort == SortType.Asc
            ? nameof(Queryable.OrderBy)
            : nameof(Queryable.OrderByDescending);

        var query = q.Provider.CreateQuery<T>(CreateSortExpressionCall(q, param, first.ColId, sortMethodName));

        for (var i = 1; i < sorts.Count; i++)
        {
            sortMethodName = sorts[i].Sort == SortType.Asc
                ? nameof(Queryable.ThenBy)
                : nameof(Queryable.ThenByDescending);
            query = query.Provider.CreateQuery<T>(CreateSortExpressionCall(query, param, sorts[i].ColId,
                sortMethodName));
        }

        return query;
    }

    private static MethodCallExpression CreateSortExpressionCall<T>(IQueryable<T> q, ParameterExpression param,
        string propertyName, string sortMethodName)
    {
        var property = Expression.Property(param, propertyName);
        var lambda = Expression.Lambda(property, param);
        return Expression.Call(typeof(Queryable),
            sortMethodName,
            new[] { q.ElementType, property.Type },
            q.Expression,
            Expression.Quote(lambda));
    }

    /// <summary>
    ///     Creates dynamic linq filtering expressions
    /// </summary>
    public static IQueryable<T> Filter<T>(this IQueryable<T> q, IDictionary<string, FilterModel> filters)
    {
        if (filters == null || !filters.Any()) return q;

        var param = Expression.Parameter(q.ElementType);

        filters.ForEach((colName, filterModel) =>
        {
            var prop = Expression.Property(param, colName);
            var lambda = Expression.Lambda(filterModel.CreateFilter(prop), param);
            q = q.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable),
                nameof(Queryable.Where),
                new[] { q.ElementType },
                q.Expression,
                Expression.Quote(lambda)));
        });

        return q;
    }

    public static IQueryable<T> HandleRequest<T>(this IQueryable<T> query, DataRequestModel<T> payload)
    {
        return query.Sort(payload.Sort)
            .Filter(payload.FilterModel)
            .Page(payload.StartRow, payload.EndRow);
    }

    public static IQueryable<T> Page<T>(this IQueryable<T> q, int startRow, int endRow)
    {
        var blockSize = endRow - startRow;
        if (blockSize > 0)
        {
            q = q.Skip(startRow).Take(blockSize);
        }

        return q;
    }

    internal static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> a)
    {
        foreach (var kvPair in dict) a(kvPair.Key, kvPair.Value);
    }
}