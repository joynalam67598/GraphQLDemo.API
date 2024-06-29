using GraphQLDemo.API.Schema.Mutaions;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course /* returen created course */;

        // want raise/pulish an event when a specific course is updated.
        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdated(string courseId, [Service] ITopicEventReceiver topicEventReciver)
        {
            var topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
            return topicEventReciver.SubscribeAsync<string, CourseResult>(topicName);
        }
    }
}
