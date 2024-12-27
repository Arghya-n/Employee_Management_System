using System.ComponentModel.DataAnnotations;

namespace EmpTaskAPI.Models
{
    public class Project
    {
        [Key]  // Explicitly defining the primary key
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        public ICollection<Task>? Tasks { get; set; }
    }

}
