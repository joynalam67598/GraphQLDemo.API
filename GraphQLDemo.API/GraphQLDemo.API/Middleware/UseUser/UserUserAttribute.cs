using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GraphQLDemo.API.Middleware.UseUser
{
    public class UserUserAttribute : ObjectFieldDescriptorAttribute
    {
        public UserUserAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Use<UserMiddleware>();
        }
    }
}
