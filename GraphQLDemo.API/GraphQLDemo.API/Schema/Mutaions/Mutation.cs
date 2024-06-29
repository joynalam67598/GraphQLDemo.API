using GraphQLDemo.API.Schema.Mutaions;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;

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
         * result format:
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

        /*query
         * mutation{
         *      updateCourse(id: Guid, name: "Chemistry", subject: SCIENCE, instructiorId: Guid){
         *        // data field we want to see.
         *        Id
         *        Name
         *        subject
         *      }
         * }
         * 
         */


        public CourseResult UpdateCourse(Guid coruseId, string name, Subject subject, Guid instructorId)
        {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == coruseId);

            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            }

            course.Name = name;
            course.Subject = subject;  
            course.InstructorId = instructorId;

            return course;
        }

        /*query
         * mutation{
         *      deleteCourse(id: Guid)
         * }
         * 
         */

        public bool DeleteCourse(Guid coruseId)
        {
            return _courses.RemoveAll(c => c.Id == coruseId) >= 1;
        }

    }
}
