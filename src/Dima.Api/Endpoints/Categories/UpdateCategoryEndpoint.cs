using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/", HandleAsync)
            .WithName("Categories: Update")
            .WithSummary("Altera uma categoria")
            .WithDescription("Altera uma categoria")
            .WithOrder(2)
            .Produces<Response<Category?>>();
    
    private static async Task<IResult> HandleAsync(ICategoryHandler handler, UpdateCategoryRequest request)
    {
        var result = await handler.UpdateAsync(request);
        
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : Results.BadRequest(result);
    }
}