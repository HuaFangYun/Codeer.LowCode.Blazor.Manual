using Codeer.LowCode.Blazor.Components.AppParts.Loading;
using Codeer.LowCode.Blazor.Components.AppParts.PageFrame;
using Codeer.LowCode.Blazor.Components.Dialog;
using Codeer.LowCode.Blazor.RequestInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;

namespace GettingStarted.CustomComponents.Services
{
  public static class ServiceInitializer
  {
    public static void AddSharedServices(this IServiceCollection services)
    {
      services.AddScoped<IAppInfoService, AppInfoService>();
      services.AddScoped<IModuleDataService, ModuleDataService>();
      services.AddScoped<IUIService, UIService>();
      services.AddScoped<ExternalServices>();
      services.AddScoped<ILogger, Logger>();
      services.AddSingleton<ModuleDialogService>();
      services.AddSingleton<MessageBoxService>();
      services.AddSingleton<LoadingService>();
      services.AddSingleton<PageFrameContext>();
      services.AddToaster(config =>
      {
        config.PositionClass = Defaults.Classes.Position.TopRight;
        config.MaximumOpacity = 100;
        config.VisibleStateDuration = 1000 * 10;
        config.ShowTransitionDuration = 10;
        config.HideTransitionDuration = 500;
      });
      services.AddScoped<ToasterEx>();
      services.AddScoped<HttpService>();
    }
  }
}
