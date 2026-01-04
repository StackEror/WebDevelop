using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace WebDevelopment.Application.Helpers;

public static class QueryProcessorExtension
{
    public static IQueryable<T> WhereAnyStringPropertyContains<T>(IQueryable<T> query, string searchTerm)
    {
        var stringProperties = typeof(T)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(string));

        if (!stringProperties.Any())
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);

        var lowerSearchTerm = searchTerm.ToLower();

        var orExpressions = stringProperties
            .Select(prop =>
            {
                var propertyAccess = Expression.Property(parameter, prop);
                var nullSafeProperty = Expression.Coalesce(propertyAccess, Expression.Constant(""));

                var propertyToLower = Expression.Call(nullSafeProperty, toLowerMethod!);

                return Expression.Call(propertyToLower, containsMethod!, Expression.Constant(lowerSearchTerm));
            })
            .Aggregate((Expression)Expression.Constant(false), Expression.OrElse);

        var lambda = Expression.Lambda<Func<T, bool>>(orExpressions, parameter);
        return query.Where(lambda);
    }

    public static IQueryable<T> OrderByColumn<T>(IQueryable<T> query, string column, string? direction = "asc")
    {
        var property = typeof(T).GetProperty(column,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property is null)
            return query;

        return direction?.ToLower() == "desc"
            ? query.OrderByDescending(e => EF.Property<object>(e!, property.Name))
            : query.OrderBy(e => EF.Property<object>(e!, property.Name));
    }
}
