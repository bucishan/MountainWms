using M.Core.Dto;
using M.Core.Entity;

namespace IServices
{
    public interface ISys_dictServices : IBaseServices<Sys_dict>
    {
        string PageList(PubParams.DictBootstrapParams bootstrap);
    }
}