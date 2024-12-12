using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class UpdateTransactionEntpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:long}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Altera uma transação")
            .WithDescription("Altera uma transação")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, UpdateTransactionRequest request,
        long id)
    {
        request.Id = id;
        request.UserId = "teste@balta.io";
        var result = await handler.UpdateAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : Results.BadRequest(result);
    }
}