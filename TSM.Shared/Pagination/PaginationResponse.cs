using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Shared.Pagination
{
    public class PaginationResponse<T>
    {
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
