using M.Core.Entity;
using System.Collections.Generic;

namespace IServices
{
    public interface ISys_menuauthServices : IBaseServices<Sys_menuauth>
    {
        /// <summary>
        /// 获取当前菜单已选择的auth数据
        /// </summary>
        /// <param name="menuPath">菜单的url地址</param>
        /// <returns></returns>
        List<Sys_auth> GetMenuSelectAuthList(string menuPath);
    }
}