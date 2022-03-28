using M.Core.Dto;
using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface IWms_inventorymoveServices : IBaseServices<Wms_inventorymove>
    {
        string PageList(PubParams.StatusBootstrapParams bootstrap);

        bool Auditin(long userId, long InventorymoveId);

        string PrintList(string InventorymoveId);
    }
}