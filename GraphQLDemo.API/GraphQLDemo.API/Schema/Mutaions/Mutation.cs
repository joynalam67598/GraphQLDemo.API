using GraphQLDemo.API.Schema.Mutaions;
using System;
using System.Collections.Generic;

namespace GraphQLDemo.API.Schema.Queries.Mutaions
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;

        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        /* execute
         * 
         * mutation{
         *      createCourse(name: "Algorithms", subject: MATHMATICS, instructiorId: Guid){
         *        // data field we want to see.
         *        Id
         *        Name
         *      }
         * }
         * 
         * 
         * result:
         * "data":{
         *      "createCourses":{
         *          "id": ---,
         *          "name": "",
         *      }
         * } 
         * 
         */

        public CourseResult CreateCourse(string name, Subject subject, Guid instructorId)
        {
            CourseResult newCourseType = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Subject = subject,
                InstructorId =  instructorId
            };

            _courses.Add(newCourseType);

            return newCourseType;
        }
    }
}
