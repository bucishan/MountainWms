using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Sys_rolemenuauthRepository : BaseRepository<Sys_rolemenuauth>, ISys_rolemenuauthRepository
    {
        public Sys_rolemenuauthRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}