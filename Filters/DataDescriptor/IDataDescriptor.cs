using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Filters.DataDescriptor;

//TODO: could return Result<T> instead of throwing exceptions
public interface IDataDescriptor<T>
{
    IDataDescriptor<T> SetPropValues(Expression<Func<T, string>> propExpression, Func<IEnumerable<string>> setFunc);

    IDataDescriptor<T> SetPropValuesAsync(Expression<Func<T, string>> propExpression,
        Func<Task<IEnumerable<string>>> setFunc);

    Task<IEnumerable<string>> Describe(string propName);
}