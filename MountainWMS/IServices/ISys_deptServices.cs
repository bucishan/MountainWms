using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface ISys_deptServices : IBaseServices<Sys_dept>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}