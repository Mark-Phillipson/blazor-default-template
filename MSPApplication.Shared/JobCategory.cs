using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public class JobCategory
    {
        public JobCategory()
        {
            Employees = new HashSet<Employee>();
        }
        public int JobCategoryId { get; set; }
        [StringLength(55)]
        [Required(ErrorMessage = "Please enter a unique Job Category name that identifies a particular job within the organisation!")]
        public string JobCategoryName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
