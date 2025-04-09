using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController: ControllerBase
    {
        private ApplicationContext context;

        public CourseController(ApplicationContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            return Ok(context.Courses.ToListAsync());
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }

        [HttpPost("post/{id}-{title}-{duration}-{description}")]
        public async void Post(int id, string title, int duration, string description)
        {
            var course = new Course { Id = id, Title = title, Duration = duration, Description = description };
            context.Courses.Add(course);

            await context.SaveChangesAsync();
        }

        [HttpPut("put/{id}-{title}-{duration}-{description}")]
        public async void Put(int id, string title, int duration, string description)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);

            if (course != null)
            {
                course.Title = title;
                course.Duration = duration;
                course.Description = description;

                await context.SaveChangesAsync();
            }
        }

        [HttpDelete("delete/{id}")]
        public async void Delete(int id)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);

            if (course != null)
            {
                context.Courses.Remove(course);

                await context.SaveChangesAsync();
            }
        }
    }
}
