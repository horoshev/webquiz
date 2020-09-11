using System;
using System.Linq.Expressions;

namespace Application.Entities
{
    public class PagingQuery<T>
    {
        public PagingQuery()
        {
        }

        public PagingQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public virtual Expression<Func<T, bool>> Condition => throw new NotImplementedException(nameof(Condition));

        public virtual Expression<Func<T, object>>? Order => ToOrder();

        private Expression<Func<T, object>>? ToOrder()
        {
            if (OrderBy is null) return null;

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, OrderBy);
            var propAsObject = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string? OrderBy { get; set; }
        public bool IsAscendingOrder { get; set; } = true;
    }
}