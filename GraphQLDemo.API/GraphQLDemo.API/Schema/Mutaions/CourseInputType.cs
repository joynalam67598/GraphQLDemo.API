using GraphQLDemo.API.Models;
using System;

namespace GraphQLDemo.API.Schema.Mutaions
{
    public class CourseInputType
    {
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
