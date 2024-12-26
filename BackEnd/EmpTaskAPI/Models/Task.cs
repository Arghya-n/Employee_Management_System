using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpTaskAPI.Models
{
    public class Task
    {
        [Key]  // Explicitly defining the primary key
        public int TaskId { get; set; }

        public int ProjectId { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string Status { get; set; }

        // Navigation property
        public Project? Project { get; set; }
    }



}
