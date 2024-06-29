using GraphQLDemo.API.Schema.Mutaions;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQLDemo.API.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course /* returen created course */;
    }
}
