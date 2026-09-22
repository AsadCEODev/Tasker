using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using TMS.Shared.Model.Setup;

namespace TMS.Shared.Model
{
    public class AppUserRole
    {
        public int Id { get; set; }
        public long UserId { get; set; } = 0;
        [ForeignKey(nameof(UserId))]
        public SetupUser? UserObject { get; set; } = null;
        public long RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public AppRole? RoleObject { get; set; } = null;
        public long? CreatedBy { get; set; } = null;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public long? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedOn { get; set; } = null;
        [NotMapped]
        public List<long> UserIds { get; set; } = new();

    }

    public class AppUserRoleDto
    {
        public int Id { get; set; } = 0;
        public List<long> UserIds { get; set; } = new();
        public string? FullName { get; set; } = null;
        public long RoleId { get; set; }
        public string? RoleName { get; set; } = String.Empty;
        public long? CreatedBy { get; set; } = null;
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public long? UpdatedBy { get; set; } = null;
        public DateTime? UpdatedOn { get; set; } = null;

    }




}
