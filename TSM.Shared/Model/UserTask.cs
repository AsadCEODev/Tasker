using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Shared.Model.Setup;

namespace TMS.Shared.Model
{
 

    [Table("UserTasks")]
    public class UserTask
    {
        [Key]
        public long Id { get; set; }

        public long? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public SetupUser? UserObj { get; set; }

        public long? TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public SetupTask? TaskObj { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }

        [StringLength(250)]
        public string? UserFileName { get; set; }

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

        public long? UserId { get; set; }

        public long? TaskId { get; set; }
        public string? UserName { get; set; }
        public string? Remarks { get; set; }
        public string? UserFileName { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
