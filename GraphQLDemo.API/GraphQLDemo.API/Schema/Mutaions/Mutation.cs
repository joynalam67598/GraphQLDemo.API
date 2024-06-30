﻿using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Mutaions;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
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
        private readonly CoursesRepository _coursesRepository;

        public Mutation(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            CourseDTO courseDTO = new CourseDTO()
            {
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId
            };

            courseDTO = await _coursesRepository.Create(courseDTO);

            CourseResult course = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            // publishing/raising the event to a topic.
            // topic is the Name of the subscription method.
            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return course;
        }

        public async Task<CourseResult> UpdateCourse(Guid courseId, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            var courseDTO = await _coursesRepository.GetCourseById(courseId);

            if (courseDTO == null)
            {
                throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            }

            courseDTO.Name = courseInputType.Name;
            courseDTO.Subject = courseInputType.Subject;
            courseDTO.InstructorId = courseInputType.InstructorId;

            courseDTO = await _coursesRepository.Update(courseDTO);

            CourseResult course = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            string updatedCourseTopic = $"{courseId}_{nameof(Subscription.CourseUpdated)}";

            await topicEventSender.SendAsync(nameof(updatedCourseTopic), course);

            return course;
        }

        public async Task<bool> DeleteCourse(Guid coruseId)
        {
            try
            {
                return await _coursesRepository.Delete(coruseId);
            }
            catch(Exception ex)
            {
                return false;
            }            
        }

    }
}
