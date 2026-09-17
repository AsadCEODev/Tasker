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
        public virtual ICollection<AppRolesScreen>? Permissions { get; set; } = new List<AppRolesScreen>();
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
        public virtual ICollection<AppRolesScreenDto>? Permissions { get; set; } = new List<AppRolesScreenDto>();
    }

    
}
