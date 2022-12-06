using System.Net.Http.Headers;
using L4D2AntiCheat.App.CurrentUser;

namespace L4D2AntiCheat.Sdk.Handlers;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly ICurrentUser _currentUser;

    public AuthorizationHeaderHandler(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = _currentUser.AccessToken;

        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", accessToken);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}