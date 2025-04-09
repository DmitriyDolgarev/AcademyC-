using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController: ControllerBase
    {
        private ApplicationContext context;

        public StudentController(ApplicationContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            return Ok(context.Students.ToListAsync());
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var student = context.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpPost("post/{id}-{firstName}-{lastName}-{age}-{address}")]
        public async void Post(int id, string firstName, string lastName, int age, string address)
        {
            var student = new Student { Id = id, FirstName = firstName, LastName = lastName, Age = age, Address = address };
            context.Students.Add(student);

            await context.SaveChangesAsync();
        }

        [HttpPut("put/{id}-{firstName}-{lastName}-{age}-{address}")]
        public async void Put(int id, string firstName, string lastName, int age, string address)
        {
            var stud = context.Students.FirstOrDefault(s => s.Id == id);

            if (stud != null)
            {
                stud.FirstName = firstName;
                stud.LastName = lastName;
                stud.Age = age;
                stud.Address = address;

                await context.SaveChangesAsync();
            }
        }

        [HttpDelete("delete/{id}")]
        public async void Delete(int id)
        {
            var student = context.Students.FirstOrDefault(s => s.Id == id);

            if (student != null)
            {
                context.Students.Remove(student);

                await context.SaveChangesAsync();
            }
        }
    }
}
