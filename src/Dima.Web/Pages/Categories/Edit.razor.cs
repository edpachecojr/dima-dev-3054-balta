using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class EditCategoryPage : ComponentBase
{
    public bool IsBusy { get; set; } = false;
    public UpdateCategoryRequest InputModel { get; set; } = new();

    [Parameter]
    public string Id { get; set; } = string.Empty;
    
    [Inject] 
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        GetCategoryByIdRequest? request = null;

        try
        {
            request = new GetCategoryByIdRequest
            {
                Id = long.Parse(Id)
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add("Parâmetro inválido", Severity.Error);
        }

        if (request is null)
            return;
        
        IsBusy = true;

        try
        {
            var response = await Handler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
                InputModel = new UpdateCategoryRequest
                {
                    Id = response.Data.Id,
                    Title = response.Data.Title,
                    Description = response.Data.Description
                };
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var response = await Handler.UpdateAsync(InputModel);
            if (response.IsSuccess)
            {
                Snackbar.Add("Categoria atualizada", Severity.Success);
                NavigationManager.NavigateTo("/categories");
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}