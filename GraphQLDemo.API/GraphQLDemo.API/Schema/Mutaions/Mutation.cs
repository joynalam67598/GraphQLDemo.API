using GraphQLDemo.API.Schema.Mutaions;
using GraphQLDemo.API.Schema.Subscriptions;
using HotChocolate;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
         * subscription{
         *      courseCreated{
         *        // data field we want to see.
         *        Id
         *        Name
         *      }
         * }
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

        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId =  courseInputType.InstructorId
            };

            _courses.Add(course);

            // publishing/raising the event to a topic.
            // topic is the Name of the subscription method.
            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return course;
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


        public CourseResult UpdateCourse(Guid courseId, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            }

            course.Name = courseInputType.Name;
            course.Subject = courseInputType.Subject;  
            course.InstructorId = courseInputType.InstructorId;


            // we will raise the event for a specific course thats we can't use method name as topic we need to use
            // Custome topic.

            string updatedCourseTopic = $"{courseId}_{nameof(Subscription.CourseUpdated)}";

            await topicEventSender.SendAsync(nameof(updatedCourseTopic), course);

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
