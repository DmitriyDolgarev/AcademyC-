using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEntities;

public partial class Course
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
