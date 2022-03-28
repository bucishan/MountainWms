using SqlSugar;
using System.Collections.Generic;
using M.Core.Dto;
using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface ISys_roleServices : IBaseServices<Sys_role>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        List<PermissionMenu> GetMenu();

        List<PermissionMenu> GetMenu(long roleId, string menuType = "menu");

        List<RoleAuthDto> GetRoleMenu(long roleId);

        DbResult<bool> Insert(Sys_role role, long userId, string[] menuId);

        DbResult<bool> Update(Sys_role role, long userId, string[] menuId);

        DbResult<bool> UpdateAuth(Sys_role role, long userId, string[] menuAuthId);
    }
}