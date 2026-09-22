using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMS.Shared.Model
{
    public class AppRolesScreen
    {
        [Key]
        public long Id { get; set; }
        public long RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public AppRole? RoleObject { get; set; } = null;
        public int ScreenId {  get; set; }
        [ForeignKey(nameof(ScreenId))]
        public AppScreen? ScreenObject { get; set; }
        public bool CanView { get; set; } = false;
        public bool CanAdd { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }

    public class AppRolesScreenDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string? RoleName { get; set; }
        public int ScreenId { get; set; }
        public string? ScreenName { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }


}
