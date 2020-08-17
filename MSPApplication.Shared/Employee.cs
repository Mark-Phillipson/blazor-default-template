using MSPApplication.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public class Employee
    {
        public Employee()
        {
            HRTasks = new HashSet<HRTask>();
        }
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name is too long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name is too long.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        private string _fullName;

        public string FullName
        {
            get
            {
                _fullName = $"{FirstName} {LastName}";
                return _fullName;
            }
        }
        [BirthdayValidator(MinimumAge = 18)]
        public DateTime BirthDate { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(70)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Street { get; set; }
        [StringLength(50)]
        public string Zip { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        [StringLength(20)]
        [Display(Name = "Phone #")]
        public string PhoneNumber { get; set; }
        public bool Smoker { get; set; }
        public bool IsOPEX { get; set; }
        public bool IsFTE { get; set; }
        [Display(Name = "Marital Status")]
        public MaritalStatus MaritalStatus { get; set; }
        public Gender Gender { get; set; }
        [StringLength(1000, ErrorMessage = "Comment length can't exceed 1000 characters.")]
        public string Comment { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public int JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        virtual public ICollection<HRTask> HRTasks { get; set; }
    }
}
