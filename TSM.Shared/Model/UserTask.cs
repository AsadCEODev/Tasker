using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Shared.Model.Setup;
using TMS.Shared.Enum;
using System.Text.Json.Serialization;

namespace TMS.Shared.Model
{
 

    [Table("UserTasks")]
    public class UserTask
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public SetupUser? UserObj { get; set; }

        public long TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public SetupTask? TaskObj { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }
        public int? UserProgress { get; set; } = 0;
        public int? StatusId { get; set; } = (int)StatusEnum.Pending;
        [StringLength(250)]
        public string? UserFileName { get; set; }
        public bool IsStart { get; set; } = false;
        public long? TaskTime { get; set; } = 0;

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class UserTaskDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TaskId { get; set; }
        public string? UserName { get; set; }
        public string? Remarks { get; set; }
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public int? UserProgress { get; set; } = 0;
        public int? StatusId { get; set; } = (int)StatusEnum.Pending;
        public string? UserFileName { get; set; }
        public bool IsStart { get; set; } = false;
        public long? TaskTime { get; set; } = 0;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }


    public class UserTaskData
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public int TagId { get; set; }
        public string? TagTitle { get; set; }
        public DateTime DueDate { get; set; }
        public string? FileName { get; set; }
        public bool IsStart { get; set; } = false;
        public long? TaskTime { get; set; }
        public long UserId { get; set; }
        public string? FullName { get; set; }
        public string? UserFileName { get; set; }
        public string? StatusTitle { get; set; }
        public int? UserProgress { get; set; }
        public int? AssignedUsersCount { get; set; }
        public string? Remarks { get; set; }
        public string? TaskDesc { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }

    }
    public class UserTaskDataDto
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public int TagId { get; set; }
        public string? TagTitle { get; set; }
        public DateTime DueDate { get; set; }
        public string? FileName { get; set; }
        public bool IsStart { get; set; } = false;
        public long? TaskTime { get; set; }
        public long UserId { get; set; }
        public string? FullName { get; set; }
        public string? UserFileName { get; set; }
        public string? StatusTitle { get; set; }
        public int? UserProgress { get; set; }
        public int? AssignedUsersCount { get; set; }
        public string? Remarks { get; set; }
        public string? TaskDesc { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
    }
}
