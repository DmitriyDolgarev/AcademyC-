using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
