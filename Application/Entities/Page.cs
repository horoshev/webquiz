using System.Collections.Generic;
using System.Linq;

namespace Application.Entities
{
    public class Page<T>
    {
        public IList<T> Data { get; set; } = new List<T>(); // Enumerable.Empty<T>();
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public static Page<T> From(PagingQuery<T> query, IEnumerable<T> data)
        {
            return new Page<T>
            {
                Data = data.ToList(),
                PageSize = query.PageSize,
                PageNumber = query.PageNumber,
            };
        }
    }
}