using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface IWms_reservoirareaServices : IBaseServices<Wms_reservoirarea>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}