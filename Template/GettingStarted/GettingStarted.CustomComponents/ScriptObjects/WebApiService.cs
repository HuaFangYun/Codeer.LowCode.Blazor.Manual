using Codeer.LowCode.Blazor.Json;
using Codeer.LowCode.Blazor.RequestInterfaces;
using Codeer.LowCode.Blazor.Script;
using GettingStarted.CustomComponents.Services;
using System.Text;

namespace GettingStarted.CustomComponents.ScriptObjects
{
  public class WebApiService
  {
    readonly HttpService _http;

    public WebApiService(HttpService http)
        => _http = http;

    [ScriptName("Get")]
    public async Task<WebApiResult> GetAsync(string url)
        => await ToWebResult(await _http.GetAsync(url));

    [ScriptName("Post")]
    public async Task<WebApiResult> PostAsync(string url, JsonObject data)
        => await ToWebResult(await _http.PostAsync(url, new StringContent(JsonConverterEx.SerializeObject(data.ToJsonableObject()), Encoding.UTF8, "application/json")));

    [ScriptName("Put")]
    public async Task<WebApiResult> PutAsync(string url, JsonObject data)
         => await ToWebResult(await _http.PutAsync(url, new StringContent(JsonConverterEx.SerializeObject(data.ToJsonableObject()), Encoding.UTF8, "application/json")));

    [ScriptName("Delete")]
    public async Task<WebApiResult> DeleteAsync(string url)
         => await ToWebResult(await _http.DeleteAsync(url));

    static async Task<WebApiResult> ToWebResult(HttpResponseMessage? response)
    {
      var result = new WebApiResult();
      if (response != null)
      {
        result.JsonObject = await GetJsonObject(response.Content);
        result.StatusCode = (int)response.StatusCode;
      }
      return result;
    }

    static async Task<JsonObject> GetJsonObject(HttpContent content)
    {
      var resultText = await content.ReadAsStringAsync();
      if (string.IsNullOrEmpty(resultText)) return new();
      else return JsonConverterEx.ToJsonObject(resultText);
    }
  }
}
