using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface IWms_storagerackServices : IBaseServices<Wms_storagerack>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}