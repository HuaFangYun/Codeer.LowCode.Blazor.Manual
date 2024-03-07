using Codeer.LowCode.Blazor.DesignLogic;
using Codeer.LowCode.Blazor.Repository.Design;

namespace GettingStarted.Server.Services
{
  internal static class DesignerService
  {
    static Dictionary<string, DesignData> _designData = new();

    internal static List<ModuleDesignAndScript> GetModulesWithoutDataSourceInfoAsync(string? appName)
        => GetDesignData(appName).ModulesWithoutDataSourceInfo.ToList();

    internal static List<PageFrameDesign> GetPageFrameDesignsAsync(string? appName)
        => GetDesignData(appName).PageFrames.ToList();

    internal static DesignData GetDesignData(string? appName)
        => DesignDataFilesUtils.GetDesignData(SystemConfig.Instance.DesignFileDirectory, appName, ref _designData);

    internal static MemoryStream? GetResource(string? appName, string resourcePath)
        => DesignDataFilesUtils.GetResource(SystemConfig.Instance.DesignFileDirectory, appName, resourcePath);
  }
}
