using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FirebaseAdminAuthentication.DependencyInjection
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FirebaseAuthenticationFunctionHandler _authenticationFunctionHandler;

        public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> optionsMonitor,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            FirebaseAuthenticationFunctionHandler authenticationFunctionHandler) : base (optionsMonitor, logger, encoder, clock)
        {
            _authenticationFunctionHandler = authenticationFunctionHandler;
        }

        protected  override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return _authenticationFunctionHandler.HandleAuthenticatieAsync(Context);
        }
    }
}
