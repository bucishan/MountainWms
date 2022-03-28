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
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class Sys_menuServices : BaseServices<Sys_menu>, ISys_menuServices
    {
        private readonly SqlSugarClient _client;
        private readonly ISys_menuRepository _repository;
        private readonly ISys_menuauthServices _menuauthServices;


        public Sys_menuServices(SqlSugarClient client,
            ISys_menuRepository repository,
            ISys_menuauthServices menuauthServices) : base(repository)
        {
            _client = client;
            _repository = repository;
            _menuauthServices = menuauthServices;
        }

        public MenuAuthDto GetMenuSelectAuthList(long menuId)
        {
            var model = new MenuAuthDto();
            var menu = _repository.QueryableToEntity(c => c.MenuId == menuId && c.IsDel == 1);
            var list = _client.Queryable<Sys_menuauth, Sys_auth>
                ((a, m) => new object[] {
                   JoinType.Left,a.AuthId==m.AuthId,
                 }).Where((a, m) => a.MenuId == menuId)
                 .Select((a, m) => new Sys_auth
                 {
                     AuthId = m.AuthId,
                     AuthName = m.AuthName,
                     AuthType = m.AuthType,
                     AuthIcon = m.AuthIcon,
                     Sort = m.Sort,
                 }).MergeTable().ToList();

            //var list = _repository.QueryableToList(c => c.MenuId == menuId);
            model = new MenuAuthDto()
            {
                MenuId = menu?.MenuId.ToString(),
                MenuName = menu?.MenuName,
                MenuIcon = menu?.MenuIcon,
                MenuUrl = menu?.MenuUrl,
                MenuParent = menu?.MenuParent,
                MenuType = menu?.MenuType,
                Sort = menu?.Sort,
                Status = menu?.Status,
                Remark = menu?.Remark,
                Children = list
            };
            return model;
        }
        public MenuInfoDto GetMenuInfo(string menuUrl)
        {
            var menu = _repository.QueryableToEntity(c => c.MenuUrl == menuUrl);
            var menuParent = _repository.QueryableToEntity(c => c.MenuId == menu.MenuParent);
            var model = new MenuInfoDto()
            {
                MenuId = menu.MenuId.ToString(),
                MenuName = menu.MenuName,
                ParentMenuId = menuParent.MenuId.ToString(),
                ParentMenuName = menuParent.MenuName,
            };
            return model;
        }

        public string PageList(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            if (bootstrap.offset != 0)
            {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            var query = _client.Queryable<Sys_menu, Sys_user, Sys_user>
                ((m, c, u) => new object[] {
                   JoinType.Left,m.CreateBy==c.UserId,
                   JoinType.Left,m.ModifiedBy==u.UserId,
                 })
                 .Where((m, c, u) => m.IsDel == 1 && m.MenuParent == -1)
                 .Select((m, c, u) => new
                 {
                     MenuId = m.MenuId.ToString(),
                     m.MenuName,
                     m.MenuIcon,
                     m.Status,
                     m.Sort,
                     CName = c.UserNickname,
                     m.CreateDate,
                     UName = u.UserNickname,
                     m.ModifiedDate
                 }).MergeTable();
            if (!bootstrap.search.IsEmpty())
            {
                query.Where((m) => m.MenuName.Contains(bootstrap.search));
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

        public string PageListDetail(string pid)
        {
            var query = _client.Queryable<Sys_menu, Sys_user, Sys_user>
                ((m, c, u) => new object[] {
                   JoinType.Left,m.CreateBy==c.UserId,
                   JoinType.Left,m.ModifiedBy==u.UserId,
                 })
                 .Where((m, c, u) => m.IsDel == 1)
                 .Select((m, c, u) => new
                 {
                     MenuId = m.MenuId.ToString(),
                     MenuOption = m.Status,
                     m.MenuName,
                     m.MenuUrl,
                     m.MenuIcon,
                     m.Status,
                     MenuParent = m.MenuParent.ToString(),
                     m.Sort,
                     CName = c.UserNickname,
                     m.CreateDate,
                     UName = u.UserNickname,
                     m.ModifiedDate
                 }).MergeTable();
            query.Where(m => m.MenuParent == pid).OrderBy(m => m.Sort, OrderByType.Asc);
            var list = query.ToList();
            return Bootstrap.GridData(list, list.Count).JilToJson();
        }

        public DbResult<bool> InsertDetail(Sys_menu menu, long userId, string[] authId)
        {
            return _client.Ado.UseTran(() =>
            {
                menu.MenuId = PubId.SnowflakeId;
                menu.MenuType = "menu";
                menu.CreateBy = userId;

                var menuId = _repository.InsertReturnEntity(menu);
                if (!menuId.IsNullT())
                {
                    var list = new List<Sys_menuauth>();
                    foreach (var item in authId)
                    {
                        list.Add(new Sys_menuauth
                        {
                            MenuAuthId = PubId.SnowflakeId,
                            AuthId = item.ToInt64(),
                            MenuId = menuId.MenuId,
                            CreateBy = userId
                        });
                    }
                    _menuauthServices.Insert(list);
                }
            });
        }

        public DbResult<bool> UpdateDetail(Sys_menu menu, long userId, string[] authId)
        {
            var list = _menuauthServices.QueryableToList(c => c.MenuId == menu.MenuId);

            var arr = list.Select(c => c.AuthId.ToString()).ToArray();

            //menuId 页面上的菜单Id;
            menu.ModifiedBy = userId;
            menu.ModifiedDate = DateTimeExt.DateTime;
            //role.RoleType = "#";
            string[] pageId = arr.Union(authId).Except(authId).ToArray(); //delete
            string[] dataId = authId.Union(arr).Except(arr).ToArray();  //insert
            return _repository.UseTran(() =>
            {
                _repository.Update(menu);
                List<long> array = new List<long>();
                if (pageId.Any())
                {
                    foreach (var item in pageId)
                    {
                        array.Add(list.Where(c => c.MenuId == menu.MenuId && c.AuthId == item.ToInt64()).SingleOrDefault().MenuAuthId);
                    }
                    _menuauthServices.Delete(array.ToArray());
                }
                if (dataId.Any())
                {
                    var menuAuthList = new List<Sys_menuauth>();
                    foreach (var item in dataId)
                    {
                        menuAuthList.Add(new Sys_menuauth
                        {
                            MenuAuthId = PubId.SnowflakeId,
                            CreateBy = userId,
                            AuthId = item.ToInt64(),
                            MenuId = menu.MenuId
                        });
                    }
                    _menuauthServices.Insert(menuAuthList);
                }
            });
        }
    }
}