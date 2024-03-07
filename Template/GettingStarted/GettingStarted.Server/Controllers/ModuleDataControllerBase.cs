using ClosedXML.Excel;
using Codeer.LowCode.Blazor.DataIO;
using Codeer.LowCode.Blazor.DesignLogic;
using Codeer.LowCode.Blazor.Json;
using Codeer.LowCode.Blazor.Repository;
using Codeer.LowCode.Blazor.Repository.Data;
using Codeer.LowCode.Blazor.Repository.Match;
using Codeer.LowCode.Blazor.Utils;
using Excel.Report.PDF;
using GettingStarted.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace GettingStarted.Server.Controllers
{
  public abstract class ModuleDataControllerBase : ControllerBase, IAsyncDisposable
  {
    protected DbAccessor DbAccess { get; }

    public ModuleDataControllerBase(DbAccessor dbAccess)
        => DbAccess = dbAccess;

    public async ValueTask DisposeAsync()
        => await DbAccess.DisposeAsync();

    [HttpGet("use_hot_reload")]
    public ValueWrapper<bool> IsUseHotReload()
        => new ValueWrapper<bool>(SystemConfig.Instance.UseHotReload);

    [HttpGet("design")]
    public async Task<DesignData> GetDesignData(string? app)
    {
      await (await CreateModuleDataService(app)).CheckAppUserCondition();
      var data = DesignerService.GetDesignData(app).JsonClone();
      data.Modules.Clear();
      return data;
    }

    [HttpPost("list")]
    public async Task<Paging<ModuleData>> GetListAsync(string? app, int? page, SearchCondition? condition)
        => await (await CreateModuleDataService(app)).GetListAsync(condition!, page ?? 0);

    [HttpGet]
    public async Task<ModuleData?> GetAsync(string? app, string? name, string? key)
        => await (await CreateModuleDataService(app)).GetAsync(name ?? string.Empty, key!);

    [HttpPost]
    public async Task<List<string>> SubmitAsync(string? app, List<ModuleSubmitData>? data)
        => await (await CreateModuleDataService(app)).SubmitAsync(data!, null);

    [HttpPost("excel_download")]
    public async Task<IActionResult> ExcelDownloadFileAsync(string? app, int? page, SearchCondition? condition)
    {
      var texts = await (await CreateModuleDataService(app)).GetTableTextsAsync(condition!, page ?? 0);
      return Ok(ExcelUtils.CreateExcelBinary(texts, "data"));
    }

    [HttpPost("excel_upload")]
    public async Task<ValueWrapper<bool>> ExcelUploadFileAsync(string? app, string? moduleName)
    {
      List<List<string>> texts = new();
      using (var memoryStream = new MemoryStream())
      {
        await Request.Body.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        using (var book = new XLWorkbook(memoryStream))
        {
          texts = book.Worksheet(1).ReadAllTexts();
        }
      }
      await (await CreateModuleDataService(app)).SubmitByTableTextsAsync(moduleName, texts, null);
      return new(true);
    }

    [HttpPost("stored_procedure")]
    public async Task<List<Dictionary<string, object?>>> ExecuteStoredProcedureAsync(string? app, string? moduleName, string? procName, List<MultiTypeValue>? arguments)
        => await (await CreateModuleDataService(app)).ExecuteStoredProcedureAsync(moduleName ?? string.Empty, procName ?? string.Empty, arguments ?? new());

    [HttpGet("resource")]
    public async Task<IActionResult> GetResourceAsync(string? app, string? resource)
    {
      await Task.CompletedTask;
      return Ok(DesignerService.GetResource(app, resource ?? string.Empty));
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFileAsync(string? app, string? moduleName, string? id, string? fieldName)
    {
      var file = await (await CreateModuleDataService(app)).FileFieldService.GetFileLocation(moduleName!, id!, fieldName!);
      return Ok(await StorageAccess.ReadFileAsync(file.Guid, file.StorageName));
    }

    [HttpPost("upload")]
    public async Task<Codeer.LowCode.Blazor.DataIO.FileInfo> UploadFileAsync(string? app, string? moduleName, string? fieldName, string? fileName)
    {
      var moduleDataService = await CreateModuleDataService(app);
      var info = moduleDataService.FileFieldService.GetFileSaveInfo(moduleName ?? string.Empty, fieldName ?? string.Empty);

      var data = new Codeer.LowCode.Blazor.DataIO.FileInfo
      {
        FileName = fileName,
        FileGuid = Guid.NewGuid(),
      };

      using (var memoryStream = new MemoryStream())
      {
        await Request.Body.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        data.FileSize = memoryStream.Length;
        await WriteTempFile(moduleDataService.FileFieldService, info.DataSourceName, info.StorageName, data.FileGuid.Value, memoryStream);
      }
      await DeleteTmpFiles(moduleDataService.FileFieldService, info.DataSourceName, info.StorageName);
      return data;
    }

    [HttpPost("mail")]
    public async Task<ValueWrapper<bool>> SendEmailAsync(GettingStarted.CustomComponents.ScriptObjects.MailRequest request)
        => new(await new MailService().SendEmailAsync(request.Address, request.Subject, request.Message));

    async Task DeleteTmpFiles(FileFieldService fileFieldService, string dataSourceName, string storageName)
    {
      var oldFiles = await fileFieldService.GetOldTemporaryFiles(dataSourceName);
      await StorageAccess.DeleteFiles(storageName, oldFiles);
      await fileFieldService.RemoveTmpFiles(dataSourceName, oldFiles);
    }

    async Task WriteTempFile(FileFieldService fileFieldService, string dataSourceName, string? storageName, Guid guid, MemoryStream memoryStream)
    {
      await fileFieldService.AddTemporaryFile(dataSourceName, guid);
      await StorageAccess.WriteFile(storageName, guid, memoryStream);
    }

    async Task<ModuleDataService> CreateModuleDataService(string? app)
    {
      var moduleDataService = new ModuleDataService(app ?? string.Empty, DbAccess, DesignerService.GetDesignData(app));
      moduleDataService.SetUserId(await GetCurrentUserIdAsync());
      return moduleDataService;
    }

    protected abstract Task<string> GetCurrentUserIdAsync();
  }
}
