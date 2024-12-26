using System.ComponentModel.DataAnnotations;

namespace EmpTaskAPI.Models
{
    public class Project
    {
        [Key]
        public String ProjectID { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


    }
}
