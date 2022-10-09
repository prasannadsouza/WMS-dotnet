using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WMSAdmin.Utility
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        public static IQueryable<T> EntityOrderBy<T>(this IQueryable<T> source, string property)
        {
            return EntityApplyOrder<T>(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(
            this IQueryable<T> source,
            string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        public static IQueryable<T> EntityOrderByDescending<T>(
           this IQueryable<T> source,
           string property)
        {
            return EntityApplyOrder<T>(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        public static IQueryable<T> EntityThenBy<T>(
            this IQueryable<T> source,
            string property)
        {
            return EntityApplyOrder<T>(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        public static IQueryable<T> EntityThenByDescending<T>(
           this IQueryable<T> source,
           string property)
        {
            return EntityApplyOrder<T>(source, property, "ThenByDescending");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(
            IQueryable<T> source,
            string property,
            string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        static IQueryable<T> EntityApplyOrder<T>(
            IQueryable<T> source,
            string property,
            string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)result;
        }


        public static IQueryable<T> SortAndPage<T>(this IOrderedQueryable<T> query, Entity.Entities.Pagination sortingPaging)
        {
            var firstPass = true;
            if (sortingPaging == null) return query;
            if (sortingPaging.SortFields == null) return query;

            sortingPaging.SortFields.RemoveAll(e => e == null);
            if (sortingPaging.SortFields.Any() == false) return query;

            foreach (var sortField in sortingPaging.SortFields)
            {
                if (firstPass)
                {
                    firstPass = false;
                    query = sortField.SortDescending == false
                                ? query.OrderBy(sortField.SortColumn) :
                                  query.OrderByDescending(sortField.SortColumn);
                }
                else
                {
                    query = sortField.SortDescending == false
                                ? query.ThenBy(sortField.SortColumn) :
                                  query.ThenByDescending(sortField.SortColumn);
                }
            }

            if (sortingPaging.RecordsPerPage <= 0) return query;

            var result = query.Skip((sortingPaging.CurrentPage - 1) *
              sortingPaging.RecordsPerPage).Take(sortingPaging.RecordsPerPage);

            return result;
        }

        public static IQueryable<T> EntitySortAndPage<T>(this IQueryable<T> query, Entity.Entities.Pagination sortingPaging)
        {
            var firstPass = true;
            if (sortingPaging == null) return query;
            if (sortingPaging.SortFields == null) return query;

            sortingPaging.SortFields.RemoveAll(e => e == null);
            if (sortingPaging.SortFields.Any() == false) return query;

            foreach (var sortField in sortingPaging.SortFields)
            {
                if (firstPass)
                {
                    firstPass = false;
                    query = sortField.SortDescending == false
                                ? query.EntityOrderBy(sortField.SortColumn) :
                                  query.EntityOrderByDescending(sortField.SortColumn);
                }
                else
                {

                    query = sortField.SortDescending == false
                                ? query.EntityThenBy(sortField.SortColumn) :
                                  query.EntityThenByDescending(sortField.SortColumn);
                }
            }

            if (sortingPaging.RecordsPerPage <= 0) return query;

            var result = query.Skip((sortingPaging.CurrentPage - 1) *
              sortingPaging.RecordsPerPage).Take(sortingPaging.RecordsPerPage);

            return result;
        }
    }
}