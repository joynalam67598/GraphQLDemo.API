using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.Models;
using HotChocolate.Resolvers;
using System.Security.Claims;
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
            if (context.ContextData.TryGetValue("ClaimsPrincipal", out object rawClaimsPrincipal)
                && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
            {
                bool emailVerified = bool.TryParse(claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED),
                                                    out bool result) ? result: false;

                User user = new User()
                {
                    Id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID),
                    UserName = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME),
                    Email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.Email),
                    EmailVerified = emailVerified,
                }
            }
            await _next(context);
        }
    }
}
