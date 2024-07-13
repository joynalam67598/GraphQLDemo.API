using HotChocolate.Resolvers;

namespace GraphQLDemo.API.Middleware.UseUser
{
    public class UserMiddleware
    {
        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public void Invoike(IMiddlewareContext context)
        {
            _next(context);
        }
    }
}
