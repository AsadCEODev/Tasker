using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMS.Shared.Model.Setup
{
    [Table("SetupUsers")]
    public class SetupUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNo { get; set; } = string.Empty;
        public string? CNIC { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string HashPassword { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }

    public class SetupUserDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNIC Number is required.")]
        public string CNIC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string HashPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare(nameof(HashPassword), ErrorMessage = "Passwords do not match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
    public class UserSummary
    {
        public long TotalUserCount { get; set; } = 0;
        public long ActiveUserCount { get; set; } = 0;
        public long InactiveUserCount { get; set; } = 0;
    }
}
