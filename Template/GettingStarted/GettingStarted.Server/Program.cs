using Codeer.LowCode.Blazor.Json;
using Codeer.LowCode.Blazor.SystemSettings;
using GettingStarted.Server.Services;
using PdfSharp.Fonts;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

GlobalFontSettings.FontResolver = new CustomFontResolver();

SystemConfig.Instance.UseHotReload = builder.Configuration.GetSection("UseHotReload").Get<bool>();
SystemConfig.Instance.DataSources = builder.Configuration.GetSection("DataSources").Get<DataSource[]>() ?? Array.Empty<DataSource>();
SystemConfig.Instance.FileStorages = builder.Configuration.GetSection("FileStorages").Get<FileStorage[]>() ?? Array.Empty<FileStorage>();
SystemConfig.Instance.DesignFileDirectory = builder.Configuration["DesignFileDirectory"] ?? string.Empty;
SystemConfig.Instance.FontFileDirectory = builder.Configuration["FontFileDirectory"] ?? string.Empty;
SystemConfig.Instance.DataSources.ToList().ForEach(e => e.ConnectionString = builder.Configuration.GetConnectionString(e.Name) ?? string.Empty);
SystemConfig.Instance.FileStorages.ToList().ForEach(e => e.ConnectionString = builder.Configuration.GetConnectionString(e.Name) ?? string.Empty);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.AddJsonConverters();
      });

if (SystemConfig.Instance.UseHotReload)
{
  builder.Services.AddSignalR();
  builder.Services.AddHostedService<FileWatcherService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

if (SystemConfig.Instance.UseHotReload)
{
  app.MapHub<HotReloadHub>("/chathub");
}

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Exception handling.
app.UseExceptionHandlerSendToFront();
app.Run();
