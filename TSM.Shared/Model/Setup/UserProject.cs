using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    public class UserProject
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public SetupUser? UserObj { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public SetupProject? ProjectObj { get; set; }
        public bool IsActive { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
    public class UserProjectDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public SetupUser? UserObj { get; set; }
        public int ProjectId { get; set; }
        public SetupProject? ProjectObj { get; set; }
        public bool IsActive { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }

    public class UserAssignProjectsDto
    {
        public List<int> ProjectIds { get; set; } = new();
        public long UserId { get; set; } = 0;
    }
}
