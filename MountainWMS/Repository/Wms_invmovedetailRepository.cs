using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Wms_invmovedetailRepository : BaseRepository<Wms_invmovedetail>, IWms_invmovedetailRepository
    {
        public Wms_invmovedetailRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}