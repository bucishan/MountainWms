using IRepository;
using IServices;

using M.Core.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace Services
{
    public class Sys_rolemenuauthServices : BaseServices<Sys_rolemenuauth>, ISys_rolemenuauthServices
    {
        private readonly ISys_rolemenuauthRepository _repository;
        private readonly SqlSugarClient _client;

        public Sys_rolemenuauthServices(SqlSugarClient client,ISys_rolemenuauthRepository repository) : base(repository)
        {
            _client = client;
            _repository = repository;
        }
        public List<Sys_auth> GetMenuSelectAuthList(string menuPath,long roleId)
        {
            var list = _client.Queryable<Sys_rolemenuauth, Sys_menuauth, Sys_auth, Sys_menu>
                ((r,m, a, n) => new object[] {
                   JoinType.Left,r.MenuAuthId==m.MenuAuthId,
                   JoinType.Left,m.AuthId==a.AuthId,
                   JoinType.Left,m.MenuId==n.MenuId,
                 }).Where((r, m, a, n) => r.RoleId == roleId && n.MenuUrl == menuPath)
                 .Select((r, m, a, n) => new Sys_auth
                 {
                     AuthId = a.AuthId,
                     AuthName = a.AuthName,
                     AuthType = a.AuthType,
                     AuthIcon = a.AuthIcon,
                     Sort = a.Sort,
                 }).MergeTable()
                 .OrderBy(c => c.Sort, OrderByType.Asc)
                 .ToList();
            return list;
        }
    }
}