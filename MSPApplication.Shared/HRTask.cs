using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public class HRTask
    {
        public int HRTaskId { get; set; }

        [Required]
        [StringLength(60)]
        public string Title { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        [Display(Name = "Assigned To")]
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public HRTaskStatus Status { get; set; }

    }
}
