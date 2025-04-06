using Microsoft.EntityFrameworkCore;
using System;

namespace AcademyEntities;
class Program
{
    public static void Main()
    {
        using (ApplicationContext db = new())
        {
            db.SeedData();

            var coursesWithStudents = db.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .ToList();

            Console.WriteLine("Курсы и их студенты: ");

            foreach (var course in coursesWithStudents)
            {
                Console.WriteLine($"Курс {course.Title}: ");

                foreach (var enrollment in course.Enrollments)
                {
                    Console.WriteLine($"Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                }
            }

            // UPDATE
            var someStudent = db.Students.FirstOrDefault();

            if (someStudent !=  null )
            {
                db.UpdateStudent(someStudent.Id, "Сергей", "Сидоров", 21, "Новосибирск");
            }

            coursesWithStudents = db.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .ToList();

            Console.WriteLine("Курсы и их студенты после обновления данных студента: ");

            foreach (var course in coursesWithStudents)
            {
                Console.WriteLine($"Курс {course.Title}: ");

                foreach (var enrollment in course.Enrollments)
                {
                    Console.WriteLine($"Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                }
            }

            //DELETE

            var studentToDelete = db.Students.FirstOrDefault();

            if (studentToDelete != null)
            {
                db.DeleteStudent(studentToDelete.Id);
            }
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            Console.WriteLine("---Явная загрузка---");

            db.SeedData();

            var course = db.Courses.FirstOrDefault();

            if (course != null)
            {
                Console.WriteLine($"Курс {course.Title}");

                db.Entry(course).Collection(c => c.Enrollments).Load();

                foreach (var enrollment in course.Enrollments)
                {
                    db.Entry(enrollment).Reference(e => e.Student).Load();
                    Console.WriteLine($"Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                }
            }
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            Console.WriteLine("---Ленивая загрузка---");

            db.SeedData();

            var course = db.Courses.FirstOrDefault();

            if (course != null)
            {
                Console.WriteLine($"Курс {course.Title}: {course.Description}, {course.Duration}");

                foreach (var enrollment in course.Enrollments)
                {
                    Console.WriteLine($"Студент: {enrollment.Student.FirstName} {enrollment.Student.LastName}");
                }
            }
        }
    }
}