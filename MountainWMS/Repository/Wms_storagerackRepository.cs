using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Wms_storagerackRepository : BaseRepository<Wms_storagerack>, IWms_storagerackRepository
    {
        public Wms_storagerackRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}