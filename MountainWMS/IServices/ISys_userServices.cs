using M.Core.Dto;
using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface ISys_userServices : IBaseServices<Sys_user>
    {
        (bool, string, SysUserDto) CheckLogin(SysUserDto dto);

        string PageList(Bootstrap.BootstrapParams bootstrap);

        void Login(long userId, string ip);
    }
}