using M.Core.Entity;
using System.Collections.Generic;

namespace IServices
{
    public interface ISys_menuauthServices : IBaseServices<Sys_menuauth>
    {
        /// <summary>
        /// ��ȡ��ǰ�˵���ѡ���auth����
        /// </summary>
        /// <param name="menuPath">�˵���url��ַ</param>
        /// <returns></returns>
        List<Sys_auth> GetMenuSelectAuthList(string menuPath);
    }
}