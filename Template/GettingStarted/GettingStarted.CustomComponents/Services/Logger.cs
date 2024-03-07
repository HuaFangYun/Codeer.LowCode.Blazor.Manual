using Blazor.DynamicJS;
using Microsoft.JSInterop;

namespace GettingStarted.CustomComponents.Services
{
  public class Logger : Codeer.LowCode.Blazor.RequestInterfaces.ILogger
  {
    readonly IJSRuntime _jsRuntime;
    readonly ToasterEx _toaster;

    public Logger(IJSRuntime js, ToasterEx toaster)
    {
      _jsRuntime = js;
      _toaster = toaster;
    }

    public async Task Log(string message)
    {
      using var js = await _jsRuntime.CreateDymaicRuntimeAsync();
      js.GetWindow().console.log(message);
    }

    public async Task Warn(string message)
    {
      _toaster.Warn(message);
      using var js = await _jsRuntime.CreateDymaicRuntimeAsync();
      js.GetWindow().console.warn(message);
    }

    public async Task Error(string message)
    {
      _toaster.Error(message);
      using var js = await _jsRuntime.CreateDymaicRuntimeAsync();
      js.GetWindow().console.error(message);
    }
  }
}
