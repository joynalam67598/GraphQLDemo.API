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
        /*
         * Query:
         * 1.
         {
            search(term: "jhon"){
              id
              ... on InstructorType{
                firstName
                lastName
                salary
              }
              ... on CourseType{
                name
              }
            }
        }
        might return course type or instructor type.
        */
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
