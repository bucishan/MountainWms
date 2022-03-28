using IRepository;
using SqlSugar;
using M.Core.Entity;

namespace Repository
{
    public class Sys_logRepository : BaseRepository<Sys_log>, ISys_logRepository
    {
        public Sys_logRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}