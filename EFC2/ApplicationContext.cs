using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEntities;

public class ApplicationContext: DbContext
{
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=sqlite20250406.db");

        optionsBuilder.UseLazyLoadingProxies().UseSqlite("DataSource=sqlite20250406.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);

    }

    public void SeedData()
    {
        if (!Students.Any() && !Courses.Any())
        {
            Student s1 = new Student { FirstName = "Vasya", LastName = "Pupkin", Age = 20, Address = "Moskow" };
            Student s2 = new Student { FirstName = "Ivan", LastName = "Ivanov", Age = 25, Address = "Bratsk" };
            Student s3 = new Student { FirstName = "Petr", LastName = "Petrov", Age = 35, Address = "Irkutsk" };

            Students.Add(s1);
            Students.Add(s2);
            Students.Add(s3);

            Course course1 = new Course { Title = "Математика", Duration = 90, Description = "Основной школьный предмет" };
            Course course2 = new Course { Title = "Физика", Duration = 100, Description = "Основной школьный предмет" };

            Courses.Add(course1);
            Courses.Add(course2);

            SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentId = s1.Id, CourseId = course1.Id }, 
                new Enrollment { StudentId = s1.Id, CourseId = course2.Id }, 
                new Enrollment { StudentId = s2.Id, CourseId = course1.Id }, 
                new Enrollment { StudentId = s3.Id, CourseId = course2.Id }
            };

            Enrollments.AddRange(enrollments);

            SaveChanges();
        }
    }

    public void DeleteStudent(int studentId)
    {
        var student = Students.Find(studentId);

        if (student != null)
        {
            Students.Remove(student);
            SaveChanges();
        }
    }

    public void UpdateStudent(int studentId, string firstName, string lastName, int age, string address)
    {
        var student = Students.Find(studentId);

        if (student != null)
        {
            student.FirstName = firstName;
            student.LastName = lastName;
            student.Age = age;
            student.Address = address; 

            SaveChanges();
        }
    }
}
