using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Web.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class ListCategoriesPage : ComponentBase
{
    public bool IsBusy { get; set; } = false;

    protected List<Category> Categories { get; private set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await Handler.GetAllAsync(request);
            
            if(result.IsSuccess)
                Categories = result.Data ?? [];
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

    public async Task OnDeleteButtonClickedAsync(long id, string title)
    {
        var parameters = new DialogParameters<CustomConfirmationDialog> 
        {
            { x => x.Title, "ATENÇÃO" },
            { x => x.Message, $"Ao prosseguir a categoria {title} será excluída. Esta é uma ação irreversível! Deseja continuar?" },
            { x => x.YesText, "Excluir" },
            { x => x.CancelText, "Cancelar" },
            { x => x.Color, Color.Error }
        };
        
        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
            FullWidth = true,
            NoHeader = false,
            CloseButton = true
        };
        
        var dialogResult = await DialogService.ShowAsync<CustomConfirmationDialog>("Atenção!", parameters, options);
        var result = await dialogResult.Result;
        if (result?.Data is true)
            await OnDeleteAsync(id, title);
        
        StateHasChanged();
    }

    private async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            await Handler.DeleteAsync(new DeleteCategoryRequest { Id = id });
            Categories.RemoveAll(c => c.Id == id);
            Snackbar.Add($"Categoria {title} excluída.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
    
    public Func<Category, bool> Filter => category =>
    {
        if(string.IsNullOrWhiteSpace(SearchTerm))
            return true;
        if(category.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        if(category.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        if(category.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    };
}