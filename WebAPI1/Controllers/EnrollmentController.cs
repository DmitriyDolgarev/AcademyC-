using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController: ControllerBase
    {
        private ApplicationContext context;

        public EnrollmentController(ApplicationContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            return Ok(context.Enrollments.ToListAsync());
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var enrollment = context.Enrollments.FirstOrDefault(e => e.EnrollmentId == id);
            if (enrollment == null)
                return NotFound();
            return Ok(enrollment);
        }

        [HttpPost("post/{id}-{studentId}-{courseId}")]
        public async void Post(int id, int studentId, int courseId)
        {
            var enrollment = new Enrollment { EnrollmentId = id, StudentId = studentId, CourseId = courseId };
            context.Enrollments.Add(enrollment);

            await context.SaveChangesAsync();
        }

        [HttpPut("put/{id}-{studentId}-{courseId}")]
        public async void Put(int id, int studentId, int courseId)
        {
            var enrollment = context.Enrollments.FirstOrDefault(e => e.EnrollmentId == id);

            if (enrollment != null)
            {
                enrollment.StudentId = studentId;
                enrollment.CourseId = courseId;

                await context.SaveChangesAsync();
            }
        }

        [HttpDelete("delete/{id}")]
        public async void Delete(int id)
        {
            var enrollment = context.Enrollments.FirstOrDefault(e => e.EnrollmentId == id);

            if (enrollment != null)
            {
                context.Enrollments.Remove(enrollment);

                await context.SaveChangesAsync();
            }
        }
    }
}
