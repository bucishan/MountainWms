using M.Core.Dto;
using M.Core.Entity;

namespace IServices
{
    public interface IWms_deviceServices : IBaseServices<Wms_device>
    {
        string PageList(PubParams.DeviceBootstrapParams bootstrap);
    }
}