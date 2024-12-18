using System.Net.Http.Json;
using System.Security.Claims;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    private bool _isAuthenticated;
    private readonly string _identityBasePath = "v1/identity";

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _isAuthenticated;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _isAuthenticated = false;
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var userInfo = await GetUser();
        if(userInfo is null)
            return new AuthenticationState(user);

        var claims = await GetClaims(userInfo);
        var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
        user = new ClaimsPrincipal(id);

        _isAuthenticated = true;
        return new AuthenticationState(user);
    }

    public void NotifyAuthenticationStateChanged()
    {
        base.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<User?> GetUser()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<User?>($"{_identityBasePath}/manage/info");
        }
        catch
        {
            return null;
        }
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(user.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
            .Select(c => new Claim(c.Value, c.Value)));

        RoleClaim[]? roles;

        try
        {
            roles = await _httpClient.GetFromJsonAsync<RoleClaim[]>($"{_identityBasePath}/roles");
        }
        catch
        {
            return claims;
        }

        foreach (var role in roles ?? [])
        {
            if(!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
                claims.Add(new Claim(ClaimTypes.Role, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
        }

        return claims;
    }
}