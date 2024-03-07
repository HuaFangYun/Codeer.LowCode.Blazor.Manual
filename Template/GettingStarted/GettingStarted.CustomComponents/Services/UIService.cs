using Blazor.DynamicJS;
using Codeer.LowCode.Blazor.Components.Dialog;
using Microsoft.JSInterop;

namespace GettingStarted.CustomComponents.Services
{
  public class UIService : Codeer.LowCode.Blazor.Components.UIService
  {
    IJSRuntime _jsRuntime;
    public UIService(
        ModuleDialogService moduleDialogService,
        MessageBoxService messageBoxService,
         IJSRuntime JSRuntime
    ) : base(moduleDialogService, messageBoxService)
        => _jsRuntime = JSRuntime;

    public override async Task<bool> DownloadFile(MemoryStream stream, string name)
    {
      using var js = await _jsRuntime.CreateDymaicRuntimeAsync();
      Download(js, name, stream.ToArray());
      return true;
    }

    static void Download(DynamicJSRuntime _js, string fileName, byte[] bin)
    {
      var window = _js!.GetWindow();
      var blob = new JSSyntax(window.Blob).New(new[] { bin }, new { type = "application/zip" });
      var url = window.URL.createObjectURL(blob);
      var anchorElement = window.document.createElement("a");
      anchorElement.href = url;
      anchorElement.download = fileName ?? "";
      anchorElement.click();
      anchorElement.remove();
      window.URL.revokeObjectURL(url);
    }
  }
}
