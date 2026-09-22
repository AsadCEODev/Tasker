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
        public string FatherName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNo { get; set; } = string.Empty;
        public string? CNIC { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        public int? DepartmentId { get; set; } = 0;
        [ForeignKey(nameof(DepartmentId))]
        public SetupDepartment? DepartmentObj { get; set; } = null;
        
        public int? DesignationId { get; set; } = 0;
        [ForeignKey(nameof(DesignationId))]
        public SetupDesignation? DesignationObj { get; set; } = null;

        [MaxLength(100)]
        public string HashPassword { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;
        public string? ProfileImagePath { get; set; } = string.Empty; 
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        [NotMapped]
        public string? Token { get; set; } = string.Empty;
    }

    public class SetupUserDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Father Name is required.")]
        public string FatherName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNIC Number is required.")]
        public string CNIC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="Please Assign Department.")]
        public int DepartmentId { get; set; } = 0;
        public SetupDepartmentDto? DepartmentObj { get; set; } = null;
        [Required(ErrorMessage = "Please Assign Designation.")]
        public int DesignationId { get; set; } = 0;
        public SetupDesignationDto? DesignationObj { get; set;} = null;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string HashPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare(nameof(HashPassword), ErrorMessage = "Passwords do not match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;
        public string? ProfileImagePath { get; set; } = string.Empty;
        [NotMapped]
        public string ConcatenatedName
        {
            get
            {
                string departmentName = DepartmentObj?.DepartmentName ?? string.Empty;

                if (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(departmentName))
                {
                    return $"{Id}-{FullName}-{departmentName}";
                }

                return $"{Id}-{FullName}";
            }
        }
    }
    public class UserSummary
    {
        public long TotalUserCount { get; set; } = 0;
        public long ActiveUserCount { get; set; } = 0;
        public long InactiveUserCount { get; set; } = 0;
    }


    public class LoginRequest
    {
        [Required(ErrorMessage ="Please Enter Valid User Name.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Valid Password.")]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public long Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }

    public class AppUsersList
    {
        public long UserId { get; set; } = 0;
        public string? UserName { get; set; } = null;
    }

    public class AppUsersListDto
    {
        public long UserId { get; set; } = 0;
        public string? UserName { get; set; } = null;
    }
}
