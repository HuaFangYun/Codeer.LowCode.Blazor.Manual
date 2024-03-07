using Codeer.LowCode.Blazor.DataIO.Db;
using Codeer.LowCode.Blazor.SystemSettings;
using Codeer.LowCode.Blazor.Utils;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace GettingStarted.Server.Services
{
  public class DbAccessor : IDbAccessor, IAsyncDisposable
  {
    bool _transactionMode;

    private class ConnectionOwner
    {
      internal bool NoNeedDispose { get; }
      internal DbConnection Connection { get; }
      internal ConnectionOwner(DbConnection connection, bool noNeedDispose)
      {
        Connection = connection;
        NoNeedDispose = noNeedDispose;
      }
    }

    readonly Dictionary<string, ConnectionOwner> _connections = new();
    readonly Dictionary<string, DbTransaction> _transactions = new();
    readonly Dictionary<string, IDbContextTransaction> _dbContextTransactions = new();
    readonly DbContext? _dbContext;

    public DbAccessor() { }
    public DbAccessor(DbContext dbContext) => _dbContext = dbContext;

    public async Task<List<DbTableDefinition>?> GetCustomDbInfoAsync(string dataSourceName)
    {
      await Task.CompletedTask;
      return null;
    }

    public DataSource GetDataSource(string dataSourceName)
        => SystemConfig.Instance.DataSources.First(e => e.Name == dataSourceName);

    public async Task CommitAsync()
    {
      foreach (var e in _transactions)
      {
        await e.Value.CommitAsync();
        await e.Value.DisposeAsync();
      }
      _transactions.Clear();
      foreach (var e in _dbContextTransactions)
      {
        await e.Value.CommitAsync();
        await e.Value.DisposeAsync();
      }
      _dbContextTransactions.Clear();
    }

    public async ValueTask DisposeAsync()
    {
      foreach (var e in _connections)
      {
        if (e.Value.NoNeedDispose) continue;
        await e.Value.Connection.DisposeAsync();
      }
      _connections.Clear();

      foreach (var e in _transactions) await e.Value.DisposeAsync();
      _transactions.Clear();

      foreach (var e in _dbContextTransactions) await e.Value.DisposeAsync();
      _dbContextTransactions.Clear();
    }

    private IDbTransaction? GetTransaction(string dataSourceName)
    {
      if (_transactions.TryGetValue(dataSourceName, out var transaction)) return transaction;
      if (_dbContextTransactions.TryGetValue(dataSourceName, out var efTransaction)) return efTransaction.GetDbTransaction();
      return null;
    }

    public async Task<int> ExecuteAsync(string dataSourceName, string query, Dictionary<string, object?> param)
    {
      var conn = GetConnection(dataSourceName);
      return await conn.ExecuteAsync(query, CreateParameter(param), GetTransaction(dataSourceName));
    }

    public async Task<string> InsertAsync(string dataSourceName, string query, Dictionary<string, object?> param)
    {
      var conn = GetConnection(dataSourceName);
      var ps = CreateParameter(param);
      var ret = (await conn.ExecuteScalarAsync<string>(query, ps, GetTransaction(dataSourceName))) ?? string.Empty;
      foreach (var e in param)
      {
        param[e.Key] = ps.Get<object>(e.Key);
      }
      return ret;
    }

    public async Task<List<IDictionary<string, object>>> QueryWithDetailTypeAsync(string dataSourceName, string query, Dictionary<string, ParamAndRawDbTypeName> param)
        => await QueryAsync(dataSourceName, query, param.ToDictionary(
            e => e.Key,
            e => e.Value.ToParameter()
        ));

    public async Task<List<IDictionary<string, object>>> QueryAsync(string dataSourceName, string query, Dictionary<string, object?> param)
    {
      var conn = GetConnection(dataSourceName);
      return (await conn.QueryAsync<object>(query, CreateParameter(param), GetTransaction(dataSourceName))).Select(e => (IDictionary<string, object>)e).ToList();
    }

    public TemporaryFilesManagement? GetTemporaryFilesManagement(string dataSourceName)
        => SystemConfig.Instance.DataSources.FirstOrDefault(e => e.Name == dataSourceName)?.TemporaryFilesManagement;

    public DataChangeHistoryManagement? GetDataChangeHistoryManagement(string dataSourceName)
        => SystemConfig.Instance.DataSources.FirstOrDefault(e => e.Name == dataSourceName)?.DataChangeHistoryManagement;

    public void StartTransaction()
        => _transactionMode = true;

    public void StartDataAccess(string dataSourceName)
        => GetConnection(dataSourceName);

    public async Task<List<Dictionary<string, object?>>> ExecuteStoredProcedureAsync(string dataSourceName, string commandText, Dictionary<string, object?> param)
    {
      var conn = GetConnection(dataSourceName);
      using (var command = conn.CreateCommand())
      {
        command.CommandText = commandText;
        foreach (var e in param)
        {
          var p = command.CreateParameter();
          p.ParameterName = e.Key;
          p.DbType = ToDbType(e.Value?.GetType());
          p.Value = e.Value;
          command.Parameters.Add(p);
        }

        using (var reader = await command.ExecuteReaderAsync())
        {
          var results = new List<Dictionary<string, object?>>();

          while (reader.Read())
          {
            results.Add(reader.ToDictionary());
          }
          return results;
        }
      }
    }

    public virtual Task<string> SubmitIdentityUserAsync(string userId, Dictionary<string, object?> columnAndValue, string? password)
        => throw new NotImplementedException();

    public virtual Task DeleteIdentityUserAsync(string userId)
        => throw new NotImplementedException();

    DbConnection GetConnection(string dataSourceName)
    {
      if (_connections.TryGetValue(dataSourceName, out var ret)) return ret.Connection;

      var dataSource = SystemConfig.Instance.DataSources.FirstOrDefault(e => e.Name == dataSourceName);
      if (dataSource == null)
      {
        throw new Exception($"{dataSourceName} not found in ({string.Join(", ", SystemConfig.Instance.DataSources.Select(e => e.Name))})");
      }

      if (dataSource.HasDbContext && _dbContext != null)
      {
        var conn = _dbContext.Database.GetDbConnection();
        conn.Open();
        if (_transactionMode)
        {
          _dbContextTransactions[dataSourceName] = _dbContext!.Database.BeginTransaction();
        }
        _connections.Add(dataSourceName, new ConnectionOwner(conn, true));
        return conn;
      }
      else
      {
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
        if (conn == null) throw new Exception("invalid data source");

        conn.Open();
        if (_transactionMode)
        {
          _transactions[dataSourceName] = conn.BeginTransaction();
        }
        _connections.Add(dataSourceName, new ConnectionOwner(conn, false));
        return conn;
      }
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

    static DbType ToDbType(Type? type)
        => type switch
        {
          null => throw new ArgumentNullException(nameof(type)),
          _ when type == typeof(byte) => DbType.Byte,
          _ when type == typeof(sbyte) => DbType.SByte,
          _ when type == typeof(short) => DbType.Int16,
          _ when type == typeof(ushort) => DbType.UInt16,
          _ when type == typeof(int) => DbType.Int32,
          _ when type == typeof(uint) => DbType.UInt32,
          _ when type == typeof(long) => DbType.Int64,
          _ when type == typeof(ulong) => DbType.UInt64,
          _ when type == typeof(float) => DbType.Single,
          _ when type == typeof(double) => DbType.Double,
          _ when type == typeof(decimal) => DbType.Decimal,
          _ when type == typeof(bool) => DbType.Boolean,
          _ when type == typeof(string) => DbType.String,
          _ when type == typeof(char) => DbType.StringFixedLength,
          _ when type == typeof(Guid) => DbType.Guid,
          _ when type == typeof(DateTime) => DbType.DateTime,
          _ when type == typeof(DateTimeOffset) => DbType.DateTimeOffset,
          _ when type == typeof(byte[]) => DbType.Binary,
          _ => DbType.Object
        };

  }

  internal static class ParamAndRawDbTypeNameExtensions
  {
    public static object? ToParameter(this ParamAndRawDbTypeName p)
    {
      /* 
       * SQL Server / Oracle char/nchar, varchar/nvarchar convert_implicit problem handling
       * With Oracle Managed DataAccess, even if IsAnsi = false, it becomes OracleDbType.Varchar and not NVarchar.
       * If you assign a VARCHAR2 parameter to a NVARCHAR2 column, conversion will be performed using SYS_OP_C2C(:param), but since it applies to the parameter, the index can be used.
       */
      /*
       * Oracle char/nchar does not properly ignore trailing spaces unless you set a fixed length parameter.
       * Blank-padded comparison semantics.
       * https://docs.oracle.com/cd/F19136_01/sqlrf/Data-Type-Comparison-Rules.html#GUID-1563C817-86BF-430B-99AB-322EE2E29187
       */
      if (p.Value is string s)
      {
        var stringFixed = p.IsStringFixedRawDbTypeName();
        // For fixed length characters, use string.length
        return new DbString
        {
          IsAnsi = p.IsUnicodeStringRawDbTypeName(),
          IsFixedLength = stringFixed,
          Value = s,
          Length = stringFixed ? s.Length : -1
        };
      }
      else return p.Value;
    }

    static bool IsUnicodeStringRawDbTypeName(this ParamAndRawDbTypeName p)
    {
      var t = p.RawDbTypeName.ToLower();
      return t == "nchar" || t == "nvarchar" || t == "nvarchar2";
    }

    static bool IsStringFixedRawDbTypeName(this ParamAndRawDbTypeName p)
    {
      var t = p.RawDbTypeName.ToLower();
      return t == "char" || t == "nchar";
    }
  }
}
