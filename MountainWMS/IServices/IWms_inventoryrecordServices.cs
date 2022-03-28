using M.Core.Dto;
using M.Core.Entity;

namespace IServices
{
    public interface IWms_inventoryrecordServices : IBaseServices<Wms_inventoryrecord>
    {
        string PageList(PubParams.InventoryBootstrapParams bootstrap);
    }
}