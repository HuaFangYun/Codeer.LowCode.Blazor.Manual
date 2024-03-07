using Codeer.LowCode.Blazor.SystemSettings;

namespace GettingStarted.Server.Services
{
  public class SystemConfig
  {
    public static SystemConfig Instance { get; set; } = new();

    public bool UseHotReload { get; set; }
    public DataSource[] DataSources { get; set; } = new DataSource[0];
    public FileStorage[] FileStorages { get; set; } = new FileStorage[0];
    public string DesignFileDirectory { get; set; } = string.Empty;
    public string FontFileDirectory { get; set; } = string.Empty;
    public MailSettings MailSettings { get; set; } = new();
  }
}
