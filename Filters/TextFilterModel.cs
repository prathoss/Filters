using System;
using System.Linq.Expressions;

namespace Filters;

public class TextFilterModel : FilterModel
{
    public TextFilterOptions Type { get; set; }
    public string Filter { get; set; }

    public override bool IsValid(MemberExpression prop)
    {
        //check type of property (text filter can only be applied to string)
        if (prop.Type != typeof(string)) return false;

        return Filter != null;
    }

    internal override Expression CreateFilter(MemberExpression prop)
    {
        var constant = Expression.Constant(Filter);

        return Type switch
        {
            TextFilterOptions.Equals => Expression.Equal(prop, constant),
            TextFilterOptions.NotEqual => Expression.NotEqual(prop, constant),
            TextFilterOptions.Contains => HandleNull(prop, Expression.Call(prop,
                typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }),
                constant)),
            TextFilterOptions.NotContains => HandleNull(prop, Expression.Not(Expression.Call(prop,
                typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }),
                constant)), true),
            TextFilterOptions.EndsWith => HandleNull(prop, Expression.Call(prop,
                typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) }),
                constant)),
            TextFilterOptions.StartsWith => HandleNull(prop, Expression.Call(prop,
                typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) }),
                constant)),
            TextFilterOptions.Empty => throw new NotImplementedException("Specify how custom filters would work"),
            _ => throw new NotImplementedException("Unrecognized text filter type"),
        };
    }

    private Expression HandleNull(MemberExpression prop, Expression notNullAction, bool shouldReturnNull = false)
    {
        var isNotNullExpression = Expression.NotEqual(prop, Expression.Constant(null));
        return shouldReturnNull
            ? Expression.OrElse(Expression.Not(isNotNullExpression), notNullAction)
            : Expression.AndAlso(isNotNullExpression, notNullAction);
    }
}

public enum TextFilterOptions
{
    Equals,
    NotEqual,
    Contains,
    NotContains,
    StartsWith,
    EndsWith,
    Empty,
}