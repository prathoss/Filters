using System;
using System.Linq.Expressions;

namespace Filters;

public class MultiFilterModel : FilterModel
{
    public FilterModel Condition1 { get; set; }

    public FilterModel Condition2 { get; set; }

    public MultiFilterOperator Operator { get; set; }

    internal override Expression CreateFilter(MemberExpression prop)
    {
        var condition1Expr = Condition1.CreateFilter(prop);
        var condition2Expr = Condition2.CreateFilter(prop);

        return Operator switch
        {
            MultiFilterOperator.And => Expression.AndAlso(condition1Expr, condition2Expr),
            MultiFilterOperator.Or => Expression.OrElse(condition1Expr, condition2Expr),
            _ => throw new NotImplementedException("Unrecognised operator"),
        };
    }

    public override bool IsValid(MemberExpression prop)
    {
        return Condition1 != null && Condition2 != null && Condition1.IsValid(prop) && Condition2.IsValid(prop);
    }
}

public enum MultiFilterOperator
{
    And,
    Or,
}