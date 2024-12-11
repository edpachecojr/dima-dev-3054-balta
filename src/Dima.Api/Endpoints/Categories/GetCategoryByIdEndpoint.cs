using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:long}", HandleAsync)
            .WithName("Categories: Get Category by Id")
            .WithSummary("Recupera uma categoria")
            .WithDescription("Recupera uma categoria")
            .WithOrder(4)
            .Produces<Response<Category?>>();
    
    private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
    {
        var request = new GetCategoryByIdRequest()
        {
            Id = id,
            UserId = "teste@balta.io"
        };
        var result = await handler.GetByIdAsync(request);
        
        return result.IsSuccess 
            ? TypedResults.Ok(result)
            : Results.BadRequest(result);
    }
}