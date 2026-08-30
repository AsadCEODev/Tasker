using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace TMS.Shared.Model.Filters
{
    public class FilterModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? QueryString { get; set; }
        public long? UserId { get; set; } = 0;
        public int? TagId { get; set; }
        public int? StatusId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class FilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? QueryString { get; set; }
        public long? UserId { get; set; } = 0;
        public int? TagId { get; set; }
        public int? StatusId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
