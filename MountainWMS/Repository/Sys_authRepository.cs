using IRepository;
using M.Core.Entity;
using SqlSugar;

namespace Repository
{
    public class Sys_authRepository : BaseRepository<Sys_auth>, ISys_authRepository
    {
        public Sys_authRepository(SqlSugarClient dbContext) : base(dbContext)
        {
        }
    }
}
