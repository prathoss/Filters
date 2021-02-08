using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Filters;

public class SetFilterModel : FilterModel
{
    public List<string> Values { get; set; }

    public override bool IsValid(MemberExpression prop)
    {
        //check type of property (set filter can only be applied to bool, enum, string and nullable boolean)
        return Values != null && ActionByType(prop,
                                  () => Values.All(x => bool.TryParse(x, out _)),
                                  () => Values.All(x => Enum.TryParse(prop.Type, x, true, out _)),
                                  //user defined set, value can not be validated at this time
                                  () => true,
                                  () => Values.All(x =>
                                      bool.TryParse(x, out _) || (x?.Equals("null",
                                          StringComparison.InvariantCultureIgnoreCase) ?? false)),
                                  () => false)
                              //filter is not empty
                              && Values.Count > 0;
    }

    private T ActionByType<T>(MemberExpression prop, Func<T> boolTypeAction, Func<T> enumTypeAction,
        Func<T> stringTypeAction,
        Func<T> nullableBoolAction, Func<T> elseAction)
    {
        if (prop.Type == typeof(bool)) return boolTypeAction();

        if (prop.Type.IsEnum) return enumTypeAction();

        if (prop.Type == typeof(string)) return stringTypeAction();

        return prop.Type == typeof(bool?)
            ? nullableBoolAction()
            : elseAction();
    }

    internal override Expression CreateFilter(MemberExpression prop)
    {
        return ActionByType(prop,
            () => CreateOrConcatenation(prop, Values.Select(bool.Parse)),
            () => CreateOrConcatenation(prop, Values.Select(x => Enum.Parse(prop.Type, x, true))),
            () => CreateOrConcatenation(prop, Values),
            () => CreateOrConcatenation(prop,
                Values.Select(x => bool.TryParse(x, out var b) ? (bool?)b : (bool?)null),
                //expression constant of bool? return expression with type bool, throwing exception (cannot compare bool and bool?)
                (fprop, x) => x.HasValue
                    ? HandleNullable(fprop, e => Expression.Equal(e, Expression.Constant(x)), false)
                    : HandleNullable(fprop, _ => Expression.Constant(false), true)),
            () => throw new ArgumentException(
                "Set filtering is only allowed on enum, bool, nullable bool and string")
        );
    }

    private Expression CreateOrConcatenation<T>(MemberExpression prop, IEnumerable<T> values)
    {
        return CreateOrConcatenation(prop, values, (prop, x) => Expression.Equal(prop, Expression.Constant(x)));
    }

    private Expression CreateOrConcatenation<T>(MemberExpression prop, IEnumerable<T> values,
        Func<MemberExpression, T, Expression> createExpression)
    {
        var list = values.ToList();

        var e = createExpression(prop, list[0]);

        switch (list.Count)
        {
            case 0:
                throw new ArgumentException("Filter must contain some values");
            case 1:
                return createExpression(prop, list[0]);
        }

        var orConcat = Expression.OrElse(
            createExpression(prop, list[0]),
            createExpression(prop, list[1]));

        for (var i = 2; i < list.Count; i++)
            orConcat = Expression.OrElse(orConcat, createExpression(prop, list[i]));

        return orConcat;
    }
}