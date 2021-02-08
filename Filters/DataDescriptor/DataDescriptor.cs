using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Filters.Extensions;

namespace Filters.DataDescriptor;

public class DataDescriptor<T> : IDataDescriptor<T>
{
    private readonly Dictionary<string, Func<Task<IEnumerable<string>>>> keyAsyncSources = new();
    private readonly Dictionary<string, Func<IEnumerable<string>>> keySources = new();

    public IDataDescriptor<T> SetPropValues(Expression<Func<T, string>> propExpression,
        Func<IEnumerable<string>> setFunc)
    {
        var propName = StandardizePropName(GetMemberName(propExpression));
        CheckForDuplicateKeys(propName);

        this.keySources.Add(propName, setFunc);

        return this;
    }

    public IDataDescriptor<T> SetPropValuesAsync(Expression<Func<T, string>> propExpression,
        Func<Task<IEnumerable<string>>> setFunc)
    {
        var propName = StandardizePropName(GetMemberName(propExpression));
        CheckForDuplicateKeys(propName);

        this.keyAsyncSources.Add(propName, setFunc);

        return this;
    }

    public async Task<IEnumerable<string>> Describe(string propName)
    {
        var standardized = StandardizePropName(propName);
        if (this.keySources.ContainsKey(standardized)) return this.keySources[standardized]();

        if (this.keyAsyncSources.ContainsKey(standardized)) return await this.keyAsyncSources[standardized]();

        var props = typeof(T).GetProperties();

        if (props.Any(x => x.Name.Equals(standardized, StringComparison.InvariantCultureIgnoreCase)))
        {
            var prop = props.First(x => x.Name.Equals(standardized, StringComparison.InvariantCultureIgnoreCase));
            var propType = prop.PropertyType;
            if (propType == typeof(bool))
                return new[] { true.ToString().ToCamelCase(), false.ToString().ToCamelCase() };

            if (propType == typeof(bool?))
                return new[] { true.ToString().ToCamelCase(), false.ToString().ToCamelCase(), "null" };

            if (propType.IsEnum)
                return Enum.GetValues(propType).OfType<object>().Select(x => x.ToString().ToCamelCase());
        }

        throw new ArgumentException(
            $"Unexpected property name {propName} only properties of type, bool, Nullable<bool>, enum and string with defined source function can be described",
            propName);
    }

    private void CheckForDuplicateKeys(string key)
    {
        if (keySources.ContainsKey(key) || keyAsyncSources.ContainsKey(key))
            throw new ArgumentException($"Duplicate source functions set for property: {key}", key);
    }

    private string StandardizePropName(string propName)
    {
        return propName.ToLowerInvariant();
    }

    private string GetMemberName<TKey>(Expression<Func<T, TKey>> propExpression)
    {
        if (!(propExpression.Body is MemberExpression))
            throw new ArgumentException("Expression must be member expression");

        return ((MemberExpression)propExpression.Body).Member.Name;
    }
}