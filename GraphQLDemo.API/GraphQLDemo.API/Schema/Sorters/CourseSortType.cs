using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Data.Sorting;

namespace GraphQLDemo.API.Schema.Sorters
{
    public class CourseSortType : SortInputType<CourseType>
    {
        protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(c => c.Id);
            descriptor.Ignore(c => c.InstructorId);

            // rename the field. added this just for demo.
            descriptor.Field(c => c.Name).Name("CourseName");

            base.Configure(descriptor);
        }
    }
}
