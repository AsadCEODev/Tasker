using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TMS.Shared.Enum;
using TMS.Shared.Pagination;

namespace TMS.Shared.Model.Setup
{
    public class SetupTask
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string TaskTitle { get; set; } = string.Empty;
        [StringLength(200)]
        public string TaskDesc { get; set; } = string.Empty;
        public int TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public SetupTag? TagObj { get; set; }
        public int? StatusId { get; set; } = (int)StatusEnum.Pending;
        [ForeignKey(nameof(StatusId))]
        public SetupStatus? StatusObj { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public SetupProject? ProjectObj { get; set; }
        public int? Progress { get; set; } = 0;
        public string? FileName { get; set; } = string.Empty;
        [NotMapped]
        public string? UserFileName { get; set; } = string.Empty;
        [NotMapped]
        public string? Remarks { get; set; } = string.Empty;
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [StringLength(30)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        [StringLength(30)]
        public string? UpdatedBy { get; set; }
        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    }

    public class SetupTaskDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Task Title is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Task Title must be between 1 and 100 characters.")]
        public string TaskTitle { get; set; } = string.Empty;
        [Required(ErrorMessage = "Task Description is required.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Task Description must be between 5 and 1000 characters.")]
        public string TaskDesc { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Tag.")]
        public int TagId { get; set; }
        public SetupTag TagObj { get; set; } = new();
        public int? StatusId { get; set; } = (int)StatusEnum.Pending;
        public SetupStatus? StatusObj { get; set; } = new();
        [Required(ErrorMessage = "Due Date is required.")]
        public DateTime DueDate { get; set; } = DateTime.Now;
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Project.")]
        public int ProjectId { get; set; }
        public SetupProject? ProjectObj { get; set; } = new();
        [Range(0, 100, ErrorMessage = "Please enter value between 0 to 100.")]
        public int? Progress { get; set; } = 0;
        public string? FileName { get; set; } = string.Empty;
        [NotMapped]
        public string? UserFileName { get; set; } = string.Empty;
        [NotMapped]
        public string? Remarks { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public List<UserTaskDto> UserTasks { get; set; } = new();
    }


    public class TaskSummary
    {
        public int TotalTasks { get; set; } = 0;
        public int TotalPendingTasks { get; set; } = 0;
        public int TotalInProcessTasks { get; set; } = 0;
        public int TotalCompletedTasks { get; set; } = 0;
        public int TotalLowTasks { get; set; } = 0;
        public int TotalMediumTasks { get; set; } = 0;
        public int TotalHighTasks { get; set; } = 0;
    }
    public class TaskSummaryDto
    {
        public int TotalTasks { get; set; } = 0;
        public int TotalPendingTasks { get; set; } = 0;
        public int TotalInProcessTasks { get; set; } = 0;
        public int TotalCompletedTasks { get; set; } = 0;
        public int TotalLowTasks { get; set; } = 0;
        public int TotalMediumTasks { get; set; } = 0;
        public int TotalHighTasks { get; set; } = 0;

    }
}
