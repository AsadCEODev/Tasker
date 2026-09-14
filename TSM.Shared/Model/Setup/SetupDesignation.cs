using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    public class SetupDesignation
    {
        [Key]
        public int Id { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedOn { get; set; } = null;
    }

    public class SetupDesignationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Designation Name.")]
        public string DesignationName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedOn { get; set; } = null;
    }


    public class vwDesignationSummaryData
    {
        public int TotalDesignations { get; set; }
        public int TotalActiveDesignations { get; set; }
        public int TotalInActiveDesignations { get; set; }
    }


    public class vwDesignationSummaryDataDto
    {
        public int TotalDesignations { get; set; }
        public int TotalActiveDesignations { get; set; }
        public int TotalInActiveDesignations { get; set; }
    }
}
