using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Filters;

public class NumberFilterModel : FilterModel
{
    public NumberFilterOptions Type { get; set; }
    public float? Filter { get; set; }
    public float? FilterTo { get; set; }
    
    protected static readonly HashSet<Type> AllowedTypes = new()
    {
        typeof(byte), typeof(sbyte),
        typeof(short),typeof(ushort),
        typeof(int), typeof(uint),
        typeof(long), typeof(ulong),
        typeof(float), typeof(double), typeof(decimal)
    };

    public override bool IsValid(MemberExpression prop)
    {
        if (!Filter.HasValue) return false;

        //check type of property (number filter can only be applied to int, long, float, double and nullables)
        if (!AllowedTypes.Contains(prop.Type) && !AllowedTypes.Contains(Nullable.GetUnderlyingType(prop.Type)))
            return false;

        return Type != NumberFilterOptions.InRange || FilterTo.HasValue;
    }

    internal override Expression CreateFilter(MemberExpression prop)
    {
        var filter = Expression.Constant(Filter!.Value);
        var filterTo = Expression.Constant(FilterTo ?? 0);

        // property is nullable and types do not match eg property is int?
        if (IsNullable(prop) && prop.Type.GetGenericArguments()[0] != Filter!.GetType())
            return HandleNullable(prop, val => ExpressionByType(val, filter, filterTo),
                Type == NumberFilterOptions.NotEqual, p => Expression.Convert(p, Filter.GetType()));

        // property is nullable and types match
        if (IsNullable(prop))
            return HandleNullable(prop, val => ExpressionByType(val, filter, filterTo),
                Type == NumberFilterOptions.NotEqual);

        // property type does not match with the filter type, for assertion must be converted to matching type
        if (prop.Type != Filter.GetType())
        {
            var convertedProp = Expression.Convert(prop, Filter.GetType());
            return ExpressionByType(convertedProp, filter, filterTo);
        }

        return ExpressionByType(prop, filter, filterTo);
    }

    private Expression ExpressionByType(Expression prop, Expression filter, Expression filterTo)
    {
        return Type switch
        {
            NumberFilterOptions.Equals => Expression.Equal(prop, filter),
            NumberFilterOptions.NotEqual => Expression.NotEqual(prop, filter),
            NumberFilterOptions.GreaterThan => Expression.GreaterThan(prop, filter),
            NumberFilterOptions.GreaterThanOrEqual => Expression.GreaterThanOrEqual(prop, filter),
            NumberFilterOptions.LessThan => Expression.LessThan(prop, filter),
            NumberFilterOptions.LessThanOrEqual => Expression.LessThanOrEqual(prop, filter),
            NumberFilterOptions.InRange => Expression.AndAlso(Expression.GreaterThan(prop, filter),
                Expression.LessThan(prop, filterTo)),
            NumberFilterOptions.Empty => throw new NotImplementedException("Specify how custom filters would work"),
            _ => throw new NotImplementedException("Unrecognized number filter type"),
        };
    }
}

public enum NumberFilterOptions
{
    Equals,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    InRange,
    Empty,
}