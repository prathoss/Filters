using System;
using System.Linq.Expressions;

namespace Filters;

public class DateFilterModel : FilterModel
{
    public DateFilterOptions Type { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public override bool IsValid(MemberExpression prop)
    {
        if (!DateFrom.HasValue) return false;

        //check type of property (date filter can only be applied to date time, date time offset and nullables)
        if (prop.Type != typeof(DateTime) && prop.Type != typeof(DateTimeOffset) &&
            prop.Type != typeof(DateTime?) && prop.Type != typeof(DateTimeOffset?))
            return false;

        return Type != DateFilterOptions.InRange || DateTo.HasValue;
    }

    internal override Expression CreateFilter(MemberExpression prop)
    {
        var from = Expression.Constant(DateFrom!.Value);
        var to = Expression.Constant(DateTo ?? DateTime.Today);

        return IsNullable(prop)
            ? HandleNullable(prop, val => ExpressionByType(val, from, to), Type == DateFilterOptions.NotEqual)
            : ExpressionByType(prop, from, to);
    }

    private Expression ExpressionByType(Expression prop, Expression from, Expression to)
    {
        return Type switch
        {
            DateFilterOptions.Equals => Expression.Equal(prop, from),
            DateFilterOptions.NotEqual => Expression.NotEqual(prop, from),
            DateFilterOptions.GreaterThan => Expression.GreaterThan(prop, from),
            DateFilterOptions.LessThan => Expression.LessThan(prop, from),
            DateFilterOptions.InRange => Expression.AndAlso(Expression.GreaterThan(prop, from),
                Expression.LessThan(prop, to)),
            DateFilterOptions.Empty => throw new NotImplementedException("Specify how custom filters would work"),
            _ => throw new NotImplementedException("Unrecognized date filter type"),
        };
    }
}

public enum DateFilterOptions
{
    Equals,
    NotEqual,
    LessThan,
    GreaterThan,
    InRange,
    Empty,
}