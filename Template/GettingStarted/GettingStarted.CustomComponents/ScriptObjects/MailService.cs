using Codeer.LowCode.Blazor.Utils;
using GettingStarted.CustomComponents.Services;

namespace GettingStarted.CustomComponents.ScriptObjects
{
  public class MailRequest
  {
    public string Address { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
  }

  public class MailService
  {
    readonly HttpService _http;

    public MailService(HttpService http) => _http = http;

    public async Task<bool> SendEmailAsync(string address, string subject, string message)
    {
      var ret = await _http.PostAsJsonAsync<MailRequest, ValueWrapper<bool>>("/api/module_data/mail", new MailRequest { Address = address, Subject = subject, Message = message });
      return ret?.Value ?? false;
    }
  }
}
