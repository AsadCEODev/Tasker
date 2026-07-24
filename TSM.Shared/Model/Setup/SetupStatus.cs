using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    public class SetupStatus
    {
        [Key] 
        public int Id { get; set; }
        public string StatusTitle { get; set; } = "Pending";
    }
    public class SetupStatusDto
    {
        public int Id { get; set; }
        public string StatusTitle { get; set; } = "Pending";
    }
}
