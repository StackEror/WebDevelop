using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using WebDevelopment.Shared.Enums;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.RequestFeatures;
using WebDevelopment.Shared.Responses;
using System.Linq.Dynamic.Core;
namespace WebDevelopment.Application.Services
{
    public class DataRequestHandlerService<SourceT, DestT>(
        IMapper mapper
        ) : IDataRequestHandlerService<SourceT, DestT>
    {
        public Response<IEnumerable<DestT>> HandleRequest(IQueryable<SourceT> source, DataRequest request)
        {
            var response = new Response<IEnumerable<DestT>>(default);
            var items = source;
            var filteredItems = items.LikeFilter(request);

            var sortedItems = filteredItems.Sort(request);

            items = sortedItems.Paginate(request);
            var list = items.ToList();
            var list2 = mapper.Map<IEnumerable<DestT>>(list);
            response = new Response<IEnumerable<DestT>>(list2);

            return response;
        }
    }

    public static class LinqExtensions
    {
        public static IQueryable<T> LikeFilter<T>(this IQueryable<T> items, DataRequest request)
        {
            DataRequest request2 = request;
            if (string.IsNullOrEmpty(request2.Keyword))
            {
                return items;
            }

            //new List<string>();
            List<string> list = (from p in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                 where p.PropertyType == typeof(string) && (request2.SearchableColumns == null || request2.SearchableColumns.Count == 0 || request2.SearchableColumns.Contains(p.Name))
                                 select p.Name).ToList();
            List<Expression<Func<T, bool>>> list2 = new List<Expression<Func<T, bool>>>();
            request2.Keyword.Trim().ToLowerInvariant();
            foreach (string item in list)
            {
                list2.Add(QueryableFunctions.GetLikeFunc<T>(item, request2.Keyword));
            }

            if (list2.Count > 0)
            {
                Expression<Func<T, bool>> predicate = list2.Aggregate(PredicateBuilder.Or);
                items = items.Where(predicate);
            }

            return items;
        }
    }
    public static class QueryableFunctions
    {
        public static Expression<Func<T, bool>> GetLikeFunc<T>(string propertyName, string filterText)
        {
            MethodInfo method = (from p in typeof(DbFunctionsExtensions).GetMethods()
                                 where p.Name == "Like"
                                 select p).First();
            ConstantExpression arg = Expression.Constant("%" + filterText + "%", typeof(string));
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "entity");
            return Expression.Lambda<Func<T, bool>>(Expression.Call(arg1: Expression.Property(parameterExpression, propertyName), method: method, arg0: Expression.Constant(EF.Functions, typeof(DbFunctions)), arg2: arg), new ParameterExpression[1] { parameterExpression });
        }
    }
    public static class PredicateBuilder
    {
        public static Expression Replace(this Expression expression, Expression searchEx, Expression replaceEx)
        {
            return new ReplaceVisitor(searchEx, replaceEx).Visit(expression);
        }

        public static Expression<Func<T, bool>> True<T>()
        {
            return (T f) => true;
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return (T f) => false;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            Expression right = expr2.Body.Replace(expr2.Parameters[0], expr1.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, right), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            Expression right = expr2.Body.Replace(expr2.Parameters[0], expr1.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, right), expr1.Parameters);
        }
    }
    internal class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression from;

        private readonly Expression to;

        public ReplaceVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            if (node != from)
            {
                return base.Visit(node);
            }

            return to;
        }
    }

    public static class DataRequestHandlerExtensions
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, DataRequest request)
        {
            if (!string.IsNullOrEmpty(request.Sort))
            {
                source = ((!(((request.SortDirection == SortDirection.Descending) ? "desc" : "asc") == "asc")) ? source.AsQueryable().OrderBy(request.Sort + " desc") : source.AsQueryable().OrderBy(request.Sort));
            }

            return source;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, DataRequest request)
        {
            if (!string.IsNullOrEmpty(request.Sort))
            {
                source = ((!(((request.SortDirection == SortDirection.Descending) ? "desc" : "asc") == "asc")) ? source.OrderBy(request.Sort + " desc") : source.OrderBy(request.Sort));
            }

            return source;
        }

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> items, DataRequest request)
        {
            int count = 0;
            if (request.PageSize > 0)
            {
                count = request.Page * request.PageSize;
            }

            return items.Skip(count).Take(request.PageSize);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> items, DataRequest request)
        {
            int count = 0;
            if (request.PageSize > 0)
            {
                count = request.Page * request.PageSize;
            }

            return items.Skip(count).Take(request.PageSize);
        }

        public static IEnumerable<T> KeywordFilter<T>(this IEnumerable<T> items, DataRequest request)
        {
            return items.AsQueryable().KeywordFilter(request);
        }

        public static IQueryable<T> KeywordFilter<T>(this IQueryable<T> items, DataRequest request)
        {
            DataRequest request2 = request;
            if (string.IsNullOrEmpty(request2.Keyword))
            {
                return items;
            }

            List<string> list = new List<string>();
            list = (from p in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where p.PropertyType == typeof(string) && (request2.SearchableColumns == null || request2.SearchableColumns.Count == 0 || request2.SearchableColumns.Contains(p.Name))
                    select p.Name).ToList();
            if (!string.IsNullOrEmpty(request2.Keyword))
            {
                List<Expression<Func<T, bool>>> list2 = new List<Expression<Func<T, bool>>>();
                request2.Keyword.Trim().ToLowerInvariant();
                foreach (string item in list)
                {
                    list2.Add(EnumerableFunctions.GetContainsFunction<T>(item, request2.Keyword));
                }

                if (list2.Count > 0)
                {
                    Expression<Func<T, bool>> predicate = list2.Aggregate(PredicateBuilder.Or);
                    items = items.Where(predicate);
                }
            }

            return items;
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, DataRequest request)
        {
            return items.AsQueryable().Filter(request);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> items, DataRequest request)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();
            list = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            List<Expression<Func<T, bool>>> list2 = new List<Expression<Func<T, bool>>>();
            if (request.Filter != null)
            {
                foreach (PropertyInfo item in list)
                {
                    if (!(request.Filter.GetType().GetProperty(item.Name) != null))
                    {
                        continue;
                    }

                    object value = request.Filter.GetType().GetProperty(item.Name).GetValue(request.Filter, null);
                    if (value != null)
                    {
                        if (item.PropertyType == typeof(string) || item.PropertyType == typeof(Guid))
                        {
                            list2.Add(EnumerableFunctions.GetContainsFunction<T>(item.Name, value.ToString()));
                        }
                        else
                        {
                            list2.Add(EnumerableFunctions.GetEqualsFunction<T>(item.PropertyType, item.Name, value));
                        }
                    }
                }
            }

            if (list2.Count > 0)
            {
                Expression<Func<T, bool>> predicate = list2.Aggregate(PredicateBuilder.And);
                items = items.Where(predicate);
            }

            return items;
        }
    }

    public class EnumerableFunctions
    {
        public static Expression<Func<T, bool>> GetContainsFunction<T>(string propertyName, string filterText)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "type");
            MemberExpression instance = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new Type[2]
            {
            typeof(string),
            typeof(StringComparison)
            });
            ConstantExpression arg = Expression.Constant(filterText, typeof(string));
            ConstantExpression arg2 = Expression.Constant(StringComparison.OrdinalIgnoreCase, typeof(StringComparison));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(instance, method, arg, arg2), new ParameterExpression[1] { parameterExpression });
        }

        public static Expression<Func<T, bool>> GetEqualsFunction<T>(string propertyName, int filterText)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "type");
            MemberExpression instance = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = typeof(int).GetMethod("Equals", new Type[1] { typeof(int) });
            ConstantExpression constantExpression = Expression.Constant(filterText, typeof(int));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(instance, method, constantExpression), new ParameterExpression[1] { parameterExpression });
        }

        public static Expression<Func<T, bool>> GetEqualsFunction<T>(Type type, string propertyName, object filterValue)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "type");
            MemberExpression instance = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = type.GetMethod("Equals", new Type[1] { type });
            ConstantExpression constantExpression = Expression.Constant(filterValue, type.IsEnum ? typeof(object) : type);
            return Expression.Lambda<Func<T, bool>>(Expression.Call(instance, method, constantExpression), new ParameterExpression[1] { parameterExpression });
        }
    }
}
