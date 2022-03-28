using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface IWms_CarrierServices : IBaseServices<Wms_Carrier>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}