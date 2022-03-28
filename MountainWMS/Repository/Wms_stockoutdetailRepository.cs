using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Wms_stockoutdetailRepository : BaseRepository<Wms_stockoutdetail>, IWms_stockoutdetailRepository
    {
        public Wms_stockoutdetailRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}