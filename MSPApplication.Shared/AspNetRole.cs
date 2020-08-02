using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        //[Display(Name="Created Date")]
        //public DateTime CreatedDate { get; set; }
        //public string Description { get; set; }

        //public string Ipaddress { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
