using SqlSugar;
using M.Core.Dto;
using M.Core.Entity;

namespace IServices
{
    public interface IWms_stockoutServices : IBaseServices<Wms_stockout>
    {
        string PageList(PubParams.StockOutBootstrapParams bootstrap);

        string PrintList(string stockInId);

        DbResult<bool> Auditin(long userId, long stockOutId);
    }
}