
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Queries;
using System;
using System.Collections.Generic;

namespace GraphQLDemo.API.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string CreatedById { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }
        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
