using System;
using System.Linq.Expressions;

namespace Filters;

public abstract class FilterModel
{
    public FilterType FilterType { get; set; }

    internal abstract Expression CreateFilter(MemberExpression prop);

    public abstract bool IsValid(MemberExpression prop);

    /// <summary>
    ///     Decides if the member is of nullable type
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    protected static bool IsNullable(MemberExpression m)
    {
        var t = m.Type;
        return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    ///     Creates expression where null values are handled
    /// </summary>
    /// <param name="prop">Objects property</param>
    /// <param name="createRight">Function that gets passed the value of nullable type</param>
    /// <param name="shouldReturnNull">For case when null is expected result from filtering</param>
    /// <returns></returns>
    protected Expression HandleNullable(Expression prop, Func<Expression, Expression> createRight,
        bool shouldReturnNull)
    {
        return HandleNullable(prop, createRight, shouldReturnNull, p => p);
    }

    /// <summary>
    ///     Creates expression where null values are handled
    /// </summary>
    /// <param name="prop">Objects property</param>
    /// <param name="createRight">Function that gets passed the value of nullable type</param>
    /// <param name="shouldReturnNull">For case when null is expected result from filtering</param>
    /// <param name="convertProp">Function to convert property before creating right expression</param>
    /// <returns></returns>
    protected Expression HandleNullable(Expression prop, Func<Expression, Expression> createRight,
        bool shouldReturnNull, Func<Expression, Expression> convertProp)
    {
        // does not have to be int, but HasValue string will stay the same
        var hasValue = Expression.Property(prop, nameof(Nullable<int>.HasValue));
        Expression propValue = Expression.Property(prop, nameof(Nullable<int>.Value));
        var right = createRight(convertProp(propValue));
        return shouldReturnNull
            ? Expression.OrElse(Expression.Not(hasValue), right)
            : Expression.AndAlso(hasValue, right);
    }
}