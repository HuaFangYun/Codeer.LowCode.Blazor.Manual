using Blazor.DynamicJS;
using Codeer.LowCode.Blazor.Components.AppParts.PageFrame;
using Codeer.LowCode.Blazor.RequestInterfaces;
using GettingStarted.CustomComponents.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GettingStarted.Client.Pages
{
  public class PageBase : ComponentBase, IAsyncDisposable
  {
    [Parameter]
    public string? AppName { get; set; }
#nullable disable
    [Inject]
    protected PageFrameContext PageFrameContext { get; set; }
    [Inject]
    protected HttpClient HttpClient { get; set; }
    [Inject]
    protected ExternalServices ExternalServices { get; set; }
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }
    [Inject]
    protected INavigationService NavigationService { get; set; }
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    [Inject]
    protected IModuleDataService ModuleDataService { get; set; }
    [Inject]
    protected IAppInfoService AppInfoService { get; set; }
    [Inject]
    protected HttpService HttpService { get; set; }

#nullable restore
    protected DynamicJSRuntime? Js { get; set; }

    protected bool IsValid { get; private set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Js = await JSRuntime.CreateDymaicRuntimeAsync();
      AppInfoService.AsImplement().OnHotReload += PageBase_OnHotReload;
    }

    private async void PageBase_OnHotReload(object? sender, EventArgs e)
    {
      StateHasChanged();
      await OnHotReloaded();
    }

    public async ValueTask DisposeAsync()
    {
      if (Js != null) await Js.DisposeAsync();
      AppInfoService.AsImplement().OnHotReload -= PageBase_OnHotReload;
    }

    protected override async Task OnParametersSetAsync()
    {
      IsValid = false;
      if (!string.IsNullOrEmpty(AppName))
      {
        await AppInfoService.AsImplement().InitializeAppAsync(AppName);
        IsValid = true;
      }
      await base.OnParametersSetAsync();
    }

    protected virtual async Task OnHotReloaded() => await Task.CompletedTask;
  }
}
