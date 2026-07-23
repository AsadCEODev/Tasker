using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    public class SetupProject
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? ProjectCity { get; set; } = string.Empty;
        public string? ProjectAddress { get; set; } = string.Empty;
        public string? ProjectLogo {  get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

    }

    public class SetupProjectDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? ProjectCity { get; set; } = string.Empty;
        public string? ProjectAddress { get; set; } = string.Empty;
        public string? ProjectLogo { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

    }
}
