using M.Core.Entity;
using System.Collections.Generic;

namespace IServices
{
    public interface ISys_rolemenuauthServices : IBaseServices<Sys_rolemenuauth>
    {
        /// <summary>
        /// ��ȡ��ǰ�˵� �û����ڽ�ɫ����Ȩ��
        /// </summary>
        /// <param name="menuPath"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<Sys_auth> GetMenuSelectAuthList(string menuPath, long roleId);
    }
}