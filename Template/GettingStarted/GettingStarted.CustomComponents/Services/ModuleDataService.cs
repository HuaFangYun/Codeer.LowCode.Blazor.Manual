using Codeer.LowCode.Blazor.DataIO;
using Codeer.LowCode.Blazor.Json;
using Codeer.LowCode.Blazor.Repository;
using Codeer.LowCode.Blazor.Repository.Data;
using Codeer.LowCode.Blazor.Repository.Match;
using Codeer.LowCode.Blazor.RequestInterfaces;
using Codeer.LowCode.Blazor.Utils;
using Microsoft.AspNetCore.Components;

namespace GettingStarted.CustomComponents.Services
{
  public class ModuleDataService : IModuleDataService
  {
    readonly NavigationManager _navigationManager;
    readonly HttpService _http;

    public ModuleDataService(HttpService http, NavigationManager navigationManager)
    {
      _http = http;
      _navigationManager = navigationManager;
    }

    public async Task<Paging<ModuleData>> GetListAsync(SearchCondition condition, int pageIndex)
        => await _http.PostAsJsonAsync<SearchCondition, Paging<ModuleData>>($"/api/module_data/list?app={GetApp()}&page={pageIndex}", condition) ?? new();

    public async Task<ModuleData?> GetAsync(string moduleDesignName, string id)
        => await _http.GetFromJsonAsync<ModuleData>($"/api/module_data?name={moduleDesignName}&key={id}&app={GetApp()}");

    public async Task<List<string>?> SubmitAsync(List<ModuleSubmitData> data)
        => await _http.PostAsJsonAsync<List<ModuleSubmitData>, List<string>>($"/api/module_data?app={GetApp()}", data);

    public async Task<WebApiResult> ExecuteStoredProcedureAsync(string moduleName, string procName, List<MultiTypeValue> arguments)
    {
      var result = new WebApiResult();

      int code = 200;
      using (var a = _http.AddChecker((code_, text) => code = (int)code_))
      {
        var value = await _http.PostAsJsonAsync<List<MultiTypeValue>, List<Dictionary<string, object?>>>(
            $"/api/module_data/stored_procedure?app={GetApp()}&moduleName={moduleName}&procName={procName}", arguments) ?? new();
        result.StatusCode = code;
        if (value != null) result.JsonObject = JsonConverterEx.ToJsonObject(JsonConverterEx.SerializeObject(value));
      }

      return result;
    }

    public async Task<Codeer.LowCode.Blazor.DataIO.FileInfo?> UploadFile(string? moduleName, string? fieldName, string fileName, StreamContent content)
        => await _http.PostContentAsJsonAsync<Codeer.LowCode.Blazor.DataIO.FileInfo>($"/api/module_data/upload?app={GetApp()}&moduleName={moduleName}&fieldName={fieldName}&fileName={fileName}", content);

    public async Task<MemoryStream?> DownloadFile(string? moduleName, string? fieldName, string? id)
    {
      var result = await _http.GetAsync($"/api/module_data/download?app={GetApp()}&moduleName={moduleName}&fieldName={fieldName}&id={id}");
      if (result == null) return null;
      return (MemoryStream)await result.Content.ReadAsStreamAsync();
    }

    public async Task<MemoryStream?> GetListByExcelFileAsync(SearchCondition condition, int pageIndex)
    {
      var result = await _http.PostAsJsonReturnHttpResponseAsync($"/api/module_data/excel_download?app={GetApp()}&page={pageIndex}", condition);
      if (result == null) return null;
      return (MemoryStream)await result.Content.ReadAsStreamAsync();
    }

    public async Task<bool> SubmitByExcelFileAsync(string? moduleName, StreamContent content)
        => (await _http.PostContentAsJsonAsync<ValueWrapper<bool>>($"/api/module_data/excel_upload?app={GetApp()}&moduleName={moduleName}", content))?.Value ?? false;

    public Task<Paging<ModuleData>> GetListWithLockAsync(SearchCondition condition) => throw new NotImplementedException("This method is not available on the client side");

    public Task<ModuleData?> GetWithLockAsync(string moduleDesignName, string id) => throw new NotImplementedException("This method is not available on the client side");

    string GetApp() => NavigationServiceBase.GetCurrentApp(_navigationManager);
  }
}
