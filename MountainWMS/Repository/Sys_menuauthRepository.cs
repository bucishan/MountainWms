using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Sys_menuauthRepository : BaseRepository<Sys_menuauth>, ISys_menuauthRepository
    {
        public Sys_menuauthRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}