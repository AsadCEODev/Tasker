using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model
{
    public class AppScreen
    {
        [Key]
        public int Id { get; set; } 
        public string ScreenName { get; set; } = string.Empty;
    }
    public class AppScreenDto
    {
       
        public int Id { get; set; }
        public string ScreenName { get; set; } = string.Empty;
    }
}
