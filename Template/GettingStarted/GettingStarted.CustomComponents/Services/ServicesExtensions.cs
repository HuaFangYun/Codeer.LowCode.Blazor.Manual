using Codeer.LowCode.Blazor.RequestInterfaces;

namespace GettingStarted.CustomComponents.Services
{
  public static class ServicesExtensions
  {
    public static AppInfoService AsImplement(this IAppInfoService src) => (AppInfoService)src;
    public static NavigationServiceBase AsImplement(this INavigationService src) => (NavigationServiceBase)src;
  }
}
