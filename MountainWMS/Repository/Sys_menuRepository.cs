using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Sys_menuRepository : BaseRepository<Sys_menu>, ISys_menuRepository
    {
        public Sys_menuRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}