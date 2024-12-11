using Dima.Api.Data;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return new Response<Category?>(category);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Category?>(null, 500, "Não foi possivel criar uma categoria.");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);
            if (category == null)
                return new Response<Category?>(null, 404, "Categoria não encontrada");
            category.Title = request.Title;
            category.Description = request.Description;
            
            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<Category?>(null, 500, "Não foi possível alterar categoria.");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        throw new NotImplementedException();
    }
}