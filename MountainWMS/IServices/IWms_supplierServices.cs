using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface IWms_supplierServices : IBaseServices<Wms_supplier>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}