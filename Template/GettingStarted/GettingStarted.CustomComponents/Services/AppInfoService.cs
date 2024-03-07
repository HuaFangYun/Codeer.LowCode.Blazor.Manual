using Codeer.LowCode.Blazor.DesignLogic;
using Codeer.LowCode.Blazor.Repository.Data;
using Codeer.LowCode.Blazor.Repository.Design;
using Codeer.LowCode.Blazor.RequestInterfaces;
using Codeer.LowCode.Blazor.Script;
using Codeer.LowCode.Blazor.Utils;
using GettingStarted.CustomComponents.ScriptObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Sotsera.Blazor.Toaster;
using System.Text;

namespace GettingStarted.CustomComponents.Services
{
  public class AppInfoService : IAppInfoService
  {
    HubConnection? _hubConnection;
    bool? _useHotReload;
    readonly NavigationManager _navigationManager;
    readonly HttpService _http;
    readonly Dictionary<string, DesignData> _designs = new();
    readonly ScriptRuntimeTypeManager _scriptRuntimeTypeManager = new();
    readonly ToasterEx _toaster;

    public ModuleData? CurrentUserData { get; private set; }

    public string CurrentUserId { get; set; } = string.Empty;

    public Guid Guid { get; set; } = Guid.NewGuid();

    public event EventHandler OnHotReload = delegate { };

    public AppInfoService(HttpService http, NavigationManager navigationManager, ToasterEx toaster)
    {
      _http = http;
      _navigationManager = navigationManager;
      _toaster = toaster;
      _scriptRuntimeTypeManager.SetCustomInjector(type => type == typeof(HttpService) ? http : null);
      _scriptRuntimeTypeManager.AddType(typeof(ScriptObjects.Excel));
      _scriptRuntimeTypeManager.AddType(typeof(ExcelCellIndex));
      _scriptRuntimeTypeManager.AddService(new WebApiService(http));
      _scriptRuntimeTypeManager.AddService(new MailService(http));
    }

    public async Task InitializeAppAsync(string appName)
    {
      await InitializeHotReloadAsync(appName);

      if (_designs.ContainsKey(appName)) return;
      DesignData desing;
      using (var a = _http.AddChecker((code_, text) => { }))
      {
        desing = await _http.GetFromJsonAsync<DesignData>($"/api/module_data/design/?app={appName}") ?? new();
      }
      _designs[appName] = desing;
      var currentUserModule = desing.ModulesWithoutDataSourceInfo.FirstOrDefault(e => e.Design.Name == desing.AppSettings.CurrentUserModuleDesignName);
      if (currentUserModule == null || string.IsNullOrEmpty(CurrentUserId)) return;
      CurrentUserData = await _http.GetFromJsonAsync<ModuleData>($"/api/module_data?name={currentUserModule.Design.Name}&key={CurrentUserId}&app={appName}");
    }

    public PageFrameDesign? GetMainPageFrameDesign(string app)
        => _designs.TryGetValue(app, out var design) ?
        design.PageFrames.FirstOrDefault(e => e.TopPageModule != null) : null;

    public ModuleDesignAndScript? GetModuleDesignAndScript(string name)
        => _designs.TryGetValue(GetApp(), out var design) ?
        design.ModulesWithoutDataSourceInfo.FirstOrDefault(e => e.Design.Name == name) : null;

    public PageFrameDesign? GetPageFrameDesign(string name)
        => _designs.TryGetValue(GetApp(), out var design) ?
        design.PageFrames.FirstOrDefault(e => e.Name == name) : null;

    public AppSettingsDesign GetApplicationSettings()
        => _designs.TryGetValue(GetApp(), out var design) ?
        design.AppSettings : new();

    string GetApp() => NavigationServiceBase.GetCurrentApp(_navigationManager);

    public ScriptRuntimeTypeManager GetScriptRuntimeTypeManager()
        => _scriptRuntimeTypeManager;

    public async Task<MemoryStream?> GetResourceAsync(string resourcePath)
    {
      if (_designs[GetApp()].BuiltInResources.TryGetValue(resourcePath.Replace("\\", "/"), out var text))
      {
        return new MemoryStream(Encoding.UTF8.GetBytes(text));
      }

      var result = await _http.GetAsync($"/api/module_data/resource?app={GetApp()}&resource={resourcePath}");
      if (result == null) return null;
      return (MemoryStream)await result.Content.ReadAsStreamAsync();
    }

    public async Task<string> GetResourceTextAsync(string resourcePath)
    {
      if (_designs[GetApp()].BuiltInResources.TryGetValue(resourcePath, out var text))
      {
        return text;
      }

      var result = await _http.GetAsync($"/api/module_data/resource?app={GetApp()}&resource={resourcePath}");
      if (result == null) return string.Empty;
      using var memory = (MemoryStream)await result.Content.ReadAsStreamAsync();
      if (memory == null) return string.Empty;
      var reader = new StreamReader(memory);
      return reader.ReadToEnd();
    }

    async Task InitializeHotReloadAsync(string appName)
    {
      if (_useHotReload == null)
      {
        _useHotReload = (await _http.GetFromJsonAsync<ValueWrapper<bool>>($"/api/module_data/use_hot_reload"))?.Value;
      }

      if (_useHotReload == true && _hubConnection == null)
      {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/chathub"))
            .Build();

        _hubConnection.On("ExecuteHotReload", async () =>
        {
          Clear();
          await InitializeAppAsync(appName);
          OnHotReload?.Invoke(this, EventArgs.Empty);
        });
        await _hubConnection.StartAsync();
      }
    }

    void Clear()
    {
      _toaster.Clear();
      Guid = Guid.NewGuid();
      _designs.Clear();
      CurrentUserId = string.Empty;
      CurrentUserData = null;
    }
  }
}
