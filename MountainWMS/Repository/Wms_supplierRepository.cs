using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Wms_supplierRepository : BaseRepository<Wms_supplier>, IWms_supplierRepository
    {
        public Wms_supplierRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}