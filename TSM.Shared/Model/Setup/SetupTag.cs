using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    public class SetupTag
    {
        [Key]
        public int Id { get; set; }
        public string TagTitle { get; set; } = string.Empty;
    }

    public class SetupTagDto
    {
        public int Id { get; set; }
        public string TagTitle { get; set; } = string.Empty;
    }
}
