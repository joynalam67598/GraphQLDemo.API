using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Instructors;
using HotChocolate;
using HotChocolate.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    

    public class CourseType
    {        
        public Guid Id { get; set; }
        [IsProjected(false)] // it will note select Name from db, even if we query for this.
        public string Name { get; set; }
        public IEnumerable<StudentType> Students { get; set; }
        public Subject Subject { get; set; }
        [IsProjected(true)] // it will always select InstructorId from db.
        public Guid InstructorId { get; set; }
        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            var  instructorDTO = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);

            return new InstructorType()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };
        }
        public string Description()
        {
            return $"{Name}: This is a course.";
        }
    }
}
