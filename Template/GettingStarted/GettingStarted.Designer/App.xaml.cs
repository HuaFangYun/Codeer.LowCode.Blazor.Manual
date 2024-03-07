using Codeer.LowCode.Blazor.Designer;
using Codeer.LowCode.Blazor.Script;
using GettingStarted.CustomComponents;
using GettingStarted.CustomComponents.ScriptObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GettingStarted.Designer
{
  public partial class App : DesignerApp
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      Services.AddSingleton<IRequestFactory, RequestFactory>();
      ScriptRuntimeTypeManager.AddType(typeof(ExcelCellIndex));
      ScriptRuntimeTypeManager.AddType(typeof(GettingStarted.CustomComponents.ScriptObjects.Excel));
      ScriptRuntimeTypeManager.AddService(new WebApiService(null!));

      InstallBundleCss("GettingStarted.CustomComponents");

      base.OnStartup(e);
      Start();
    }
  }
}
