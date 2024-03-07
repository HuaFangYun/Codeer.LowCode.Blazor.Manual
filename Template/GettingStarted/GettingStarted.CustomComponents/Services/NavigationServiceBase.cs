using Codeer.LowCode.Blazor.Repository.Design;
using Codeer.LowCode.Blazor.RequestInterfaces;
using Codeer.LowCode.Blazor.Script;
using Microsoft.AspNetCore.Components;

namespace GettingStarted.CustomComponents.Services
{
  public abstract class NavigationServiceBase : INavigationService
  {
    readonly NavigationManager _navigationManager;

    public abstract bool CanLogout { get; }
    public abstract Task Logout();

    public NavigationServiceBase(NavigationManager navigationManager) => _navigationManager = navigationManager;

    [ScriptHide]
    public string GetTopPageUrl() => $"/{GetCurrentApp()}";

    [ScriptHide]
    public string GetUrl(PageLink pageLink)
    {
      var pageFrame = string.IsNullOrEmpty(pageLink.PageFrame) ? GetCurrentPageFrame() : pageLink.PageFrame;
      var url = pageLink.ModuleLayoutType == ModuleLayoutType.List ?
              $"/{GetCurrentApp()}/{pageFrame}/{pageLink.Module}" :
              $"/{GetCurrentApp()}/{pageFrame}/{pageLink.Module}/-";
      if (!string.IsNullOrEmpty(pageLink.Id)) url += $"/{pageLink.Id}";
      return url;
    }
    [ScriptHide]
    public string GetListUrl(string module) => $"/{GetCurrentApp()}/{GetCurrentPageFrame()}/{module}";
    [ScriptHide]
    public string GetListUrl(string pageFrame, string module) => $"/{GetCurrentApp()}/{pageFrame}/{module}";
    [ScriptHide]
    public string GetModuleUrl(string module) => $"/{GetCurrentApp()}/{GetCurrentPageFrame()}/{module}/-";
    [ScriptHide]
    public string GetModuleUrl(string pageFrame, string module) => $"/{GetCurrentApp()}/{pageFrame}/{module}/-";
    [ScriptHide]
    public string GetModuleDataUrl(string module, string id) => $"/{GetCurrentApp()}/{GetCurrentPageFrame()}/{module}/{id}";
    [ScriptHide]
    public string GetModuleDataUrl(string pageFrame, string module, string id) => $"/{GetCurrentApp()}/{pageFrame}/{module}/{id}";

    public void NavigateToList(string module) => _navigationManager.NavigateTo(GetListUrl(module));
    public void NavigateToList(string pageFrame, string module) => _navigationManager.NavigateTo(GetListUrl(pageFrame, module));
    public void NavigateToModule(string module) => _navigationManager.NavigateTo(GetModuleUrl(module));
    public void NavigateToModule(string pageFrame, string module) => _navigationManager.NavigateTo(GetModuleUrl(pageFrame, module));
    public void NavigateToModuleData(string module, string id) => _navigationManager.NavigateTo(GetModuleDataUrl(module, id));
    public void NavigateToModuleData(string pageFrame, string module, string id) => _navigationManager.NavigateTo(GetModuleDataUrl(pageFrame, module, id));
    public void ReplaceToModuleData(string module, string id) => _navigationManager.NavigateTo(GetModuleDataUrl(module, id), false, true);

    [ScriptHide]
    public void ReplaceToTopPage(string app, PageFrameDesign mainLayout)
    {
      if (mainLayout.TopPageModuleLayoutType == ModuleLayoutType.List) _navigationManager.NavigateTo(GetListUrl(app, mainLayout.Name, mainLayout.TopPageModule ?? string.Empty), false, true);
      else _navigationManager.NavigateTo(GetModuleUrl(app, mainLayout.Name, mainLayout.TopPageModule ?? string.Empty), false, true);
    }

    internal static string GetCurrentApp(NavigationManager mgr)
    {
      var x = mgr.Uri.Substring(mgr.BaseUri.Length).Split('/');
      return x.Length < 1 || string.IsNullOrEmpty(x[0]) ? string.Empty : x[0];
    }

    internal static string GetCurrentPageFrame(NavigationManager mgr)
    {
      var x = mgr.Uri.Substring(mgr.BaseUri.Length).Split('/');
      return x.Length < 2 ? string.Empty : x[1];
    }

    string GetCurrentPageFrame() => GetCurrentPageFrame(_navigationManager);
    string GetCurrentApp() => GetCurrentApp(_navigationManager);
    static string GetListUrl(string app, string pageFrame, string module) => $"/{app}/{pageFrame}/{module}";
    static string GetModuleUrl(string app, string pageFrame, string module) => $"/{app}/{pageFrame}/{module}/-";
  }
}
