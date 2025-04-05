using Microsoft.EntityFrameworkCore;
using System;

namespace AcademyEntities;
class Program
{
    public static void Main()
    {
        using (ApplicationContext db = new())
        {
            var courses = db.Courses.ToList();

            int maxId = courses.Select(c => c.Id).Max();


            db.Courses.Add(new Course
            {
                Id = ++maxId,
                Title = "Математика",
                Duration = 80,
                Description = "Предмет для студентов"
            });

            db.Courses.Add(new Course
            {
                Id = ++maxId,
                Title = "Физика",
                Duration = 100,
                Description = "Предмет для студентов"
            });

            db.SaveChanges();

            var newCourses = db.Courses.ToList();

            foreach (var course in newCourses)
            {
                Console.WriteLine($"[Предмет] { course.Id }- { course.Title }: { course.Duration }, { course.Description }");
            }
        }
    }
}