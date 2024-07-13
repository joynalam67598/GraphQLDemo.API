using Bogus;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Filters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
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
        
        // apply pagination in db query. for this we don't need to pass the size to repository to use in take()
        // if we return querible to hotchocolate it do this for us.

        /* filtering query
         * 
         * GetCourses(first: 3, where: {
         *      or: [ -> || operator
         *          {
         *              name: {
         *                  contains: "A"
         *              },
         *              subject: {
         *                  eq: SCIENCE
         *              }
         *          
         *          }
         *      ]
         *      
         * },
         *      order: {
         *          name: ASC
         *      }
         * )
         * {
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

        // order of the bellow attribute matter
        [UseDbContext(typeof(SchoolDBContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)] /* enable pagination */
        [UseProjection]
        [UseFiltering(typeof(CourseFilterType))]
        [UseSorting] // directly applied to database query.
        public async Task<IQueryable<CourseType>> GetPaginatedCourses([ScopedService] SchoolDBContext contex)
        {
            var CourseDTOs = await _courseRepository.GetAllCourse();

            return contex.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatedById = c.CreatedById
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
                InstructorId = courseDTO.InstructorId,
                CreatedById = courseDTO.CreatedById
            };
        }


        /*
         * Query:
         * {
         * }
        */


        // might return course type or instructor type.
        public async Task<IEnumerable<ISearchResultType>> Search(string term, [ScopedService] SchoolDBContext context)
        {
            IEnumerable<CourseType> courses = await context.Courses
                .Where(c => c.Name.ToLower().Contains(term.ToLower()))
                .Select(c => new CourseType()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId,
                    CreatedById = c.CreatedById
                }).ToListAsync();

            IEnumerable<InstructorType> instructors = await context.Instructors
                .Where(c => c.FirstName.ToLower().Contains(term.ToLower()) || c.LastName.ToLower().Contains(term.ToLower()))
                .Select(c => new InstructorType()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Salary = c.Salary
                }).ToListAsync();

            return new List<ISearchResultType>()
                .Concat(courses)
                .Concat(instructors);
        }

        [GraphQLDeprecated("This query is deprecated.")]
        public string Instructions => "Query Type.";
    }
}
