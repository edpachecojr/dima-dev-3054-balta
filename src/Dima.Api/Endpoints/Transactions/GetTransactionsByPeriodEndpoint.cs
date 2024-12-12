using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get All Transactions")
            .WithSummary("Busca todas as transações de um periodo")
            .WithDescription("Busca todas as transações de um periodo")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction?>>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, 
        [FromQuery] int pageNumber,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] int pageSize = Configuration.DefaultPageSize
        )
    {
        var request = new GetTransactionsByPeriodRequest()
        {
            UserId = "teste@balta.io",
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartDate = startDate,
            EndDate = endDate
        };
        var result = await handler.GetByPeriod(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : Results.BadRequest(result);
    }
}