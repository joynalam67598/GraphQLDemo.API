using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Courses
{
    // we can directry use DB contex in our schema but we are keeping a top layer over dbcontex to keep is seprate.
    public class CoursesRepository
    {
        // always have new db contex factory.
        private readonly IDbContextFactory<SchoolDBContext> _dBContexFactory;

        public CoursesRepository(IDbContextFactory<SchoolDBContext> dbContexFactory)
        {
            _dBContexFactory = dbContexFactory;
        }

        public async Task<CourseDTO> Create(CourseDTO course)
        {
            using(SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                contex.Courses.Add(course);
                await contex.SaveChangesAsync();

                return course;
            }
        }

        public async Task<CourseDTO> GetCourseById(Guid courseId)
        {
            using (SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                var course = await contex.Courses
                    .FirstOrDefaultAsync(c => c.Id == courseId);
                return course;
            }
        }

        public async Task<CourseDTO> Update(CourseDTO course)
        {
            using (SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                contex.Courses.Update(course);
                await contex.SaveChangesAsync();

                return course;
            }
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourse()
        {
            using (SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                return await contex.Courses.ToListAsync();
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                CourseDTO course = new CourseDTO()
                {
                    Id = id
                };

                contex.Courses.Remove(course);
                return await contex.SaveChangesAsync() > 0;
            }
        }
    }
}
