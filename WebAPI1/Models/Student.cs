using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
