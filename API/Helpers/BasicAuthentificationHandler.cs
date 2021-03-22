using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodeBlock.Engine.API.Services;
using NodeBlock.Engine.API.Entities;

namespace NodeBlock.Engine.API.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IWalletService _walletService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IWalletService walletService)
            : base(options, logger, encoder, clock)
        {
            _walletService = walletService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await Task.Run(() => {
                if (!Request.Headers.ContainsKey("Secret-Key"))
                    return AuthenticateResult.Fail("Missing SecretKey Header");

                try
                {
                    var keyHeader = AuthenticationHeaderValue.Parse(Request.Headers["Secret-Key"]);
                    var credentialBytes = Convert.FromBase64String(keyHeader.ToString());
                    var key = System.Text.Encoding.UTF8.GetString(credentialBytes);

                    if (!(key == Environment.GetEnvironmentVariable("secretKey")))
                    {
                        return AuthenticateResult.Fail("Invalid Authorization Header");
                    }
                }
                catch
                {
                    return AuthenticateResult.Fail("Invalid Key Header");
                }

                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "API"),
                new Claim(ClaimTypes.Name, ""),
            };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            });
            
        }
    }
}
