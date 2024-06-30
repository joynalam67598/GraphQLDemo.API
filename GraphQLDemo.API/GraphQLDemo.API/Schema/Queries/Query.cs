using Bogus;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Courses;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        private readonly CoursesRepository _courseRepository;
        public Query(CoursesRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // Resolver
        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            var CourseDTOs = await _courseRepository.GetAllCourse();

            return CourseDTOs.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                Instructor = new InstructorType()
                {
                    Id = c.InstructorId,
                    FirstName = c.Instructor.FirstName,
                    LastName = c.Instructor.LastName,
                    Salary = c.Instructor.Salary
                }
            });
        }

        // Resolver
        public async Task<CourseType> GetCourseByIdAsync (Guid id)
        {
            var courseDTO = await _courseRepository.GetCourseById(id);
            return  new CourseType()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                Instructor = new InstructorType()
                {
                    Id = courseDTO.InstructorId,
                    FirstName = courseDTO.Instructor.FirstName,
                    LastName = courseDTO.Instructor.LastName,
                    Salary = courseDTO.Instructor.Salary
                }
            };
        }


        [GraphQLDeprecated("This query is deprecated.")]
        public string Instructions => "Query Type.";
    }
}
