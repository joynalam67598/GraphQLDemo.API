using Bogus;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
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

        /*
         * GetCourses(first: 3, after: -> weher we want to start, before: -> until){
         *      edges { -> courses data.
         *          node { -> individual courses.
         *              id
         *              name
         *              subject
         *              instructor{}
         *          }
         *          cursor -> specify the starting of the pasination
         *      }
         *      pageInfo{
         *          endCursor
         *      }
         *      totalCount
         *  }
         */

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)] /* enable pagination */
        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            var CourseDTOs = await _courseRepository.GetAllCourse();

            return CourseDTOs.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        // apply pagination in db query. for this we don't need to pass the size to repository to use in take()
        // if we return querible to hotchocolate it do this for us.

        [UseDbContext(typeof(SchoolDBContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)] /* enable pagination */
        public async Task<IQueryable<CourseType>> GetPaninatedCourses([ScopedService] SchoolDBContext contex)
        {
            var CourseDTOs = await _courseRepository.GetAllCourse();

            return contex.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
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
                InstructorId = courseDTO.InstructorId
                
            };
        }


        [GraphQLDeprecated("This query is deprecated.")]
        public string Instructions => "Query Type.";
    }
}
