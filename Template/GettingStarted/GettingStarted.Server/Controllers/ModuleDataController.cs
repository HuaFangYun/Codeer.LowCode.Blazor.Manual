using GettingStarted.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace GettingStarted.Server.Controllers
{
  [ApiController]
  [Route("api/module_data")]
  public class ModuleDataController : ModuleDataControllerBase
  {
    public ModuleDataController()
        : base(new DbAccessor()) { }

    protected override async Task<string> GetCurrentUserIdAsync()
    {
      await Task.CompletedTask;
      return string.Empty;
    }
  }
}
