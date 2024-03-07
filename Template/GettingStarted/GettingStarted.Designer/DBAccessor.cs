using Codeer.LowCode.Blazor.DataIO.Db;
using Codeer.LowCode.Blazor.SystemSettings;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace GettingStarted.Designer
{
  public class DbAccessor : IDbAccessor
  {
    DataSource[] _dataSources;
    public DbAccessor(DataSource[] dataSources)
        => _dataSources = dataSources;

    public async Task<List<DbTableDefinition>?> GetCustomDbInfoAsync(string dataSourceName)
    {
      await Task.CompletedTask;
      return null;
    }

    public async Task<List<IDictionary<string, object>>> QueryWithDetailTypeAsync(string dataSourceName, string query, Dictionary<string, ParamAndRawDbTypeName> param)
        => await QueryAsync(dataSourceName, query, param.ToDictionary(e => e.Key, e => e.Value.Value));

    public async Task<List<IDictionary<string, object>>> QueryAsync(string dataSourceName, string query, Dictionary<string, object?> param)
    {
      using var conn = GetConnection(dataSourceName);
      return (await conn.QueryAsync<object>(query, CreateParameter(param))).Select(e => (IDictionary<string, object>)e).ToList();
    }

    DbConnection GetConnection(string dataSourceName)
    {
      var dataSource = _dataSources.First(e => e.Name == dataSourceName);
      if (dataSource == null) throw new Exception();

      DbConnection conn;
      switch (dataSource.DataSourceType)
      {
        case DataSourceType.SQLServer:
          conn = new SqlConnection(dataSource.ConnectionString);
          break;
        case DataSourceType.PostgreSQL:
          conn = new NpgsqlConnection(dataSource.ConnectionString);
          break;
        case DataSourceType.Oracle:
          conn = new OracleConnection(dataSource.ConnectionString);
          break;
        case DataSourceType.SQLite:
          conn = new SQLiteConnection(dataSource.ConnectionString);
          break;
        default: throw new Exception();
      }
      conn.Open();
      return conn;
    }

    static DynamicParameters CreateParameter(Dictionary<string, object?> param)
    {
      var dst = new DynamicParameters();
      foreach (var e in param)
      {
        var val = e.Value;
        if (val is DateOnly dateOnly) val = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
        if (val is TimeOnly timeOnly) val = new TimeSpan(timeOnly.Hour, timeOnly.Minute, timeOnly.Second);
        dst.Add(e.Key, val);
      }
      return dst;
    }

    public DataSource GetDataSource(string dataSourceName) => _dataSources.First(e => e.Name == dataSourceName);
    public Task CommitAsync() => throw new NotImplementedException();
    public Task<int> ExecuteAsync(string dataSourceName, string query, Dictionary<string, object?> param) => throw new NotImplementedException();
    public Task<string> InsertAsync(string dataSourceName, string query, Dictionary<string, object?> param) => throw new NotImplementedException();
    public TemporaryFilesManagement? GetTemporaryFilesManagement(string dataSourceName) => throw new NotImplementedException();
    public void StartTransaction() => throw new NotImplementedException();
    public Task<string> SubmitIdentityUserAsync(string userId, Dictionary<string, object?> columnAndValue, string? password) => throw new NotImplementedException();
    public Task DeleteIdentityUserAsync(string userId) => throw new NotImplementedException();
    public void StartDataAccess(string dataSourceName) => throw new NotImplementedException();
    public Task<List<Dictionary<string, object?>>> ExecuteStoredProcedureAsync(string moduleDesignName, string procName, Dictionary<string, object?> param) => throw new NotImplementedException();
    public DataChangeHistoryManagement? GetDataChangeHistoryManagement(string dataSourceName) => throw new NotImplementedException();
  }
}
