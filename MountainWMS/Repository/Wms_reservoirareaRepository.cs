using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Wms_reservoirareaRepository : BaseRepository<Wms_reservoirarea>, IWms_reservoirareaRepository
    {
        public Wms_reservoirareaRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}