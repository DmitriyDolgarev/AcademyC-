using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEntities;

public partial class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; }
}
