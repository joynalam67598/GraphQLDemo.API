using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Data.Filters;

namespace GraphQLDemo.API.Schema.Filters
{
    public class CourseFilterType : FilterInputType<CourseType>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
        {
            // remove students from filter option from course type

            descriptor.Ignore(c => c.Students);
            base.Configure(descriptor);
        }
    }
}
