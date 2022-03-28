using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Wms_warehouseRepository : BaseRepository<Wms_warehouse>, IWms_warehouseRepository
    {
        public Wms_warehouseRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}