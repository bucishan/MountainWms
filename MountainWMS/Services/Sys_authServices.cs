using IRepository;
using IServices;
using SqlSugar;
using System;
using M.Core.Dto;
using M.Core.Entity;
using M.Utils.Extensions;
using M.Utils.Json;
using M.Utils.Table;
using M.Utils.Pub;
using System.Linq;
using System.Collections.Generic;

namespace Services
{
    public class Sys_authServices : BaseServices<Sys_auth>, ISys_authServices
    {
        private readonly SqlSugarClient _client;
        private readonly ISys_authRepository _repository;

        public Sys_authServices(SqlSugarClient client, ISys_authRepository repository) : base(repository)
        {
            _client = client;
            _repository = repository;
        }

        public string PageList(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            if (bootstrap.offset != 0)
            {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            var query = _client.Queryable<Sys_auth, Sys_user, Sys_user>
                ((m, c, u) => new object[] {
                   JoinType.Left,m.CreateBy==c.UserId,
                   JoinType.Left,m.ModifiedBy==u.UserId,
                 })
                 .Where((m, c, u) => m.IsDel == 1)
                 .Select((m, c, u) => new
                 {
                     AuthId = m.AuthId.ToString(),
                     m.AuthName,
                     m.AuthType,
                     m.AuthIcon,
                     m.Sort,
                     m.Status,
                     CName = c.UserNickname,
                     m.CreateDate,
                     UName = u.UserNickname,
                     m.ModifiedDate
                 }).MergeTable();
            if (!bootstrap.search.IsEmpty())
            {
                query.Where((m) => m.AuthName.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query.Where(m => m.CreateDate > bootstrap.datemin.ToDateTimeB() && m.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query.OrderBy($"MergeTable.{bootstrap.sort} desc");
            }
            else
            {
                query.OrderBy($"MergeTable.{bootstrap.sort} asc");
            }
            var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }
    }
}