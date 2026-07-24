using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        public int? StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        public SetupStatus? StatusObj { get; set; }

        public DateTime DueDate { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public SetupProject? ProjectObj { get; set; }

        public long? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public SetupUser? UserObj { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [StringLength(30)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; }

        [StringLength(30)]
        public string? UpdatedBy { get; set; }
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
        [NotMapped]
        public SetupTag TagObj { get; set; } = new();
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Status.")]
        public int? StatusId { get; set; }
        [NotMapped]
        public SetupStatus? StatusObj { get; set; } = new();

        [Required(ErrorMessage = "Due Date is required.")]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a Project.")]
        public int ProjectId { get; set; }
        [NotMapped]
        public SetupProject? ProjectObj { get; set; } = new();
        public long? UserId { get; set; } = null;
        [NotMapped]
        public SetupUser? UserObj { get; set; } = new();
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
