using M.Core.Entity;
using System.Collections.Generic;

namespace IServices
{
    public interface ISys_rolemenuauthServices : IBaseServices<Sys_rolemenuauth>
    {
        /// <summary>
        /// 获取当前菜单 用户所在角色可用权限
        /// </summary>
        /// <param name="menuPath"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Sys_auth> GetMenuSelectAuthList(string menuPath, long roleId);
    }
}