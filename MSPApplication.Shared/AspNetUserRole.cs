using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public partial class AspNetUserRole
    {
        [Display(Name ="User")]
        public string UserId { get; set; }
        [Display(Name = "Application Role")]
        public string RoleId { get; set; }

        public AspNetRole Role { get; set; }
        public AspNetUser User { get; set; }
    }
}
