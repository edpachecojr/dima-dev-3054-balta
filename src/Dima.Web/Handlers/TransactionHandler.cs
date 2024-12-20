using System.Net.Http.Json;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "v1/transactions";

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível criar sua transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível atualizar sua transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _httpClient.DeleteAsync($"{BaseUrl}/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Não foi possível excluir sua transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        => await _httpClient.GetFromJsonAsync<Response<Transaction?>>($"{BaseUrl}/{request.Id}")
           ?? new Response<Transaction?>(null, 400, "Não foi possível recuperar a transação");

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriod(GetTransactionsByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";
        var startDate = request.StartDate is not null
            ? request.StartDate.Value.ToString(format)
            : DateTime.Now.GetStartOfMonth().ToString(format);
        
        var endDate = request.EndDate is not null
            ? request.EndDate.Value.ToString(format)
            : DateTime.Now.GetEndOfMonth().ToString(format);
        
        var url = $"{BaseUrl}?startDate={startDate}&endDate={endDate}";
        
        return await _httpClient.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url)
               ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível excluir sua transação");;
    }
}