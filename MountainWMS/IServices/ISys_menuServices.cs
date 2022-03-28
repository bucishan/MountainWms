using M.Core.Dto;
using M.Core.Entity;
using M.Utils.Table;
using SqlSugar;

namespace IServices
{
    public interface ISys_menuServices : IBaseServices<Sys_menu>
    {

        string PageList(Bootstrap.BootstrapParams bootstrap);

        string PageListDetail(string pid);

        DbResult<bool> InsertDetail(Sys_menu menu, long userId, string[] authId);

        DbResult<bool> UpdateDetail(Sys_menu menu, long userId, string[] authId);

        /// <summary>
        /// ��ȡ��ǰ�˵���ѡ���auth����
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        MenuAuthDto GetMenuSelectAuthList(long menuId);

        /// <summary>
        /// ��ȡ��ǰ�˵���Ϣ
        /// </summary>
        /// <param name="menuUrl"></param>
        /// <returns></returns>
        MenuInfoDto GetMenuInfo(string menuUrl);
    }
}