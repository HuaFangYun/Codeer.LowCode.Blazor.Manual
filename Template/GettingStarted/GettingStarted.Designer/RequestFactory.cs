using Codeer.LowCode.Blazor.DataIO.Db;
using Codeer.LowCode.Blazor.Designer;
using Codeer.LowCode.Blazor.SystemSettings;

namespace GettingStarted.Designer
{
  public class RequestFactory : IRequestFactory
  {
    public IDbAccessor CreateDbAccess(DataSource[] dataSources) => new DbAccessor(dataSources);
  }
}
