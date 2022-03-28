using SqlSugar;
using System.Collections.Generic;
using M.Core.Dto;
using M.Core.Entity;
using M.Utils.Table;

namespace IServices
{
    public interface ISys_authServices : IBaseServices<Sys_auth>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);


    }
}