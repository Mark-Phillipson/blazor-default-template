using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared.ViewModels
{
    public partial class User
    {
        public User()
        {
            //AspNetUserClaims = new HashSet<AspNetUserClaim>();
            //AspNetUserLogins = new HashSet<AspNetUserLogin>();
            //AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [Display(Name = "Two Factor Enabled")]
        public bool TwoFactorEnabled { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //public ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        //public ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        //public ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
