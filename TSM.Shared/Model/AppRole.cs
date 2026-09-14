using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model
{
    public class AppRole
    {
        [Key]
        public long Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public long? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedOn { get; set; } = null;
    }

    public class AppRoleDto
    {
        [Key]
        public long Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public long? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedOn { get; set; } = null;
    }
}
