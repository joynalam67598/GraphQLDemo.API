using HotChocolate;
using System;
using System.Collections.Generic;

namespace GraphQLDemo.API.Schema
{
    public enum Subject
    {
        Mathmatics,
        Science,
        History
    }

    public class CourseType
    {        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StudentType> Students { get; set; }
        public Subject Subject { get; set; }
        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }
    }
}
