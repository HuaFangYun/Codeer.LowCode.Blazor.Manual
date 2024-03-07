using GettingStarted.CustomComponents.Services;
using Microsoft.AspNetCore.Components;

namespace GettingStarted.Client
{
  public class NavigationService : NavigationServiceBase
  {
    public NavigationService(NavigationManager nav) : base(nav) { }
    public override bool CanLogout => false;
    public override async Task Logout() => await Task.CompletedTask;
  }
}
