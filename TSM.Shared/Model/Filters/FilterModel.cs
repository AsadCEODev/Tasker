using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Shared.Model.Filters
{
    public class FilterModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? QueryString { get; set; }
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
        public int? TagId { get; set; }
        public int? StatusId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
