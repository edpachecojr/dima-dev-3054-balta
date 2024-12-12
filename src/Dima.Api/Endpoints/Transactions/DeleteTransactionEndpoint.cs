using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id:long}", HandleAsync)
            .WithName("Transaction: Delete")
            .WithSummary("Remove uma transação")
            .WithDescription("Remove uma transação")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();
    
    private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var request = new DeleteTransactionRequest()
        {
            Id = id,
            UserId = "teste@balta.io"
        };
        var result = await handler.DeleteAsync(request);
        
        return result.IsSuccess 
            ? TypedResults.NoContent()
            : Results.BadRequest(result);
    }
}