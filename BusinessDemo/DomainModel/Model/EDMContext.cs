using TelChina.TRF.Domain.Core;
using TelChina.TRF.Util.Common;

namespace TelChina.TRF.Demo.DomainModel.Model
{
    public static class DemoRepositoryContext
    {
        public static IRepositoryContext GetCurrentContext()
        {
            var connStr = ConfigHelper.GetConfigValue("ConnectionName");
            return EDMRepositoryContext.GetCurrentContext(new DemoEntities(connStr));
        }
    }
}
