using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Services
{
    public class SchoolDBContext : DbContext
    {
        public SchoolDBContext(DbContextOptions<SchoolDBContext> options)
            :base(options) { }

        public DbSet<CourseDTO> Courses { get; set; }
        public DbSet<StudentDTO> Students { get; set; }
        public DbSet<InstructorDTO> Instructors { get; set; }
    }
}
