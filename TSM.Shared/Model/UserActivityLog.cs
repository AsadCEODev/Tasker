using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model
{
    public class UserActivityLog
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserAction { get; set; } = string.Empty; 
        public string Details { get; set; } = string.Empty; 
        public DateTime ActivityDateTime { get; set; } = DateTime.Now;
    }
}
