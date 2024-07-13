using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Middleware.UseUser
{
    public class UserMiddleware
    {
        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {
            await _next(context);
        }
    }
}
