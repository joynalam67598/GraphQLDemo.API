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
         *      createCourse(courseInput: {
         *          name: "Algorithms",
         *          subject: MATHMATICS,
         *          instructiorId: Guid
         *      }){
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

        public CourseResult CreateCourse(CourseInputType courseInputType)
        {
            CourseResult newCourseType = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId =  courseInputType.InstructorId
            };

            _courses.Add(newCourseType);

            return newCourseType;
        }

        /*query
         * mutation{
         *      updateCourse(id: Guid, courseInput: { name: "Chemistry", subject: SCIENCE, instructiorId: Guid }){
         *        // data field we want to see.
         *        Id
         *        Name
         *        subject
         *      }
         * }
         * 
         */


        public CourseResult UpdateCourse(Guid courseId, CourseInputType courseInputType)
        {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            }

            course.Name = courseInputType.Name;
            course.Subject = courseInputType.Subject;  
            course.InstructorId = courseInputType.InstructorId;

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
