using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    private const string BaseUrl = "v1/categories";

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao criar uma categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao atualizar a categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _httpClient.DeleteAsync($"{BaseUrl}/{request.Id}");
        return result.StatusCode == System.Net.HttpStatusCode.NoContent
            ? new Response<Category?>(null, 204, "Categoria excluída com sucesso")
            : new Response<Category?>(null, 400, "Falha ao excluir categoria");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        => await _httpClient.GetFromJsonAsync<Response<Category?>>($"{BaseUrl}/{request.Id}")
           ?? new Response<Category?>(null, 400, "Não foi possível obter a Categoria");

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request) =>
        await _httpClient.GetFromJsonAsync<PagedResponse<List<Category>>>($"{BaseUrl}?pageNumber=1")
        ?? new PagedResponse<List<Category>>(null, 400, "Não foi possível obter Categorias");
}