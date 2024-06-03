using FastEndpoints;
using FastEndpointTest.Auth;

namespace FastEndpointTest.Authentication
{
    public class PermissionsEndpoint : EndpointWithoutRequest
    {

        public override void Configure()
        {
            Get("authentication/permissions");
            AccessControl( // Show all available permissions in alfa tools
                "Authentication_Show_Permissions",
                Apply.ToThisEndpoint,
                "Authentication"
                );
        }

        public override async Task HandleAsync(CancellationToken c)
        {

            if (Allow.Health.Count() == 0)
            {
                // Simple example
            }

            await SendAsync(new object()).ConfigureAwait(false);
        }

    }
}
