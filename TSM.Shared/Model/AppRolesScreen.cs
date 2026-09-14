using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model
{
    public class AppRolesScreen
    {
        [Key]
        public long Id { get; set; }
        public long RoleId { get; set; }
        public int ScreenId {  get; set; }
        public bool CanView { get; set; } = false;
        public bool CanAdd { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }

    public class AppRolesScreenDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public int ScreenId { get; set; }
        public bool CanView { get; set; } = false;
        public bool CanAdd { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }
}
