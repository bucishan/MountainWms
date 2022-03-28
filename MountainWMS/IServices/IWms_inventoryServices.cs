using M.Core.Dto;
using M.Core.Entity;

namespace IServices
{
    public interface IWms_inventoryServices : IBaseServices<Wms_inventory>
    {
        string PageList(PubParams.InventoryBootstrapParams bootstrap);

        string SearchInventory(PubParams.InventoryBootstrapParams bootstrap);
    }
}