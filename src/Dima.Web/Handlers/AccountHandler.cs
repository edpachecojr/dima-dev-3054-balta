using System.Net.Http.Json;
using System.Text;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    private readonly string _identityBasePath = "v1/identity";
    
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync($"{_identityBasePath}/login?useCookies=true", request);
        return result.IsSuccessStatusCode 
            ? new Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!") 
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível realizar login");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync($"{_identityBasePath}/register", request);
        return result.IsSuccessStatusCode 
            ? new Response<string>("Cadastro realizado com sucesso!", 201, "Cadastro realizado com sucesso!") 
            : new Response<string>(null, (int)result.StatusCode, "Não foi possível realizar seu cadastro");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _httpClient.PostAsJsonAsync($"{_identityBasePath}/logout", emptyContent);
    }
}