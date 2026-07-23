using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    [Table("SetupTasks")]
    public class SetupTask
    {
        [Key]
        public long Id { get; set; }

        [StringLength(100)]
        public string TaskTitle { get; set; } = string.Empty;

        [StringLength(200)]
        public string TaskDesc { get; set; } = string.Empty;
        public int TagId { get; set; } = 0;
        public int StatusId { get; set; } = 0;
        public DateTime DueDate { get; set; }  = DateTime.Now;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [StringLength(30)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; } = null;

        [StringLength(30)]
        public string? UpdatedBy { get; set; } = null;
    }
    public class SetupTaskDto
    {

        public long Id { get; set; }

        public string TaskTitle { get; set; } = string.Empty;
        public string TaskDesc { get; set; } = string.Empty;
        public int TagId { get; set; } = 0;
        public int StatusId { get; set; } = 0;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; } = null;
        public string? UpdatedBy { get; set; } = null;
    }
}
