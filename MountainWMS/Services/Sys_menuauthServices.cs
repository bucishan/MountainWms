using IRepository;
using IServices;
using M.Core.Dto;
using M.Core.Entity;
using SqlSugar;
using System.Collections.Generic;

namespace Services
{
    public class Sys_menuauthServices : BaseServices<Sys_menuauth>, ISys_menuauthServices
    {
        private readonly ISys_menuauthRepository _repository;
        private readonly SqlSugarClient _client;


        public Sys_menuauthServices(SqlSugarClient client, ISys_menuauthRepository repository) : base(repository)
        {
            _client = client;
            _repository = repository;
        }
        public List<Sys_auth> GetMenuSelectAuthList(string menuPath)
        {
            var list = _client.Queryable<Sys_menuauth, Sys_auth, Sys_menu>
                ((m, a, n) => new object[] {
                   JoinType.Left,m.AuthId==a.AuthId,
                   JoinType.Left,m.MenuId==n.MenuId,
                 }).Where((m, a, n) => n.MenuUrl == menuPath)
                 .Select((m, a, n) => new Sys_auth
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