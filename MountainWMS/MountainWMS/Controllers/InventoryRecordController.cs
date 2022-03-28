using IServices;
using Microsoft.AspNetCore.Mvc;
using M.Core.Dto;
using M.NetCore.Attributes;
using M.NetCore.NetCoreApp;
using M.Utils.Pub;

namespace MountainWMS.Controllers
{
    public class InventoryRecordController : BaseController
    {
        private readonly IWms_inventoryrecordServices _inventoryrecordServices;

        public InventoryRecordController(
            IWms_inventoryrecordServices inventoryrecordServices
            )
        {
            _inventoryrecordServices = inventoryrecordServices;
        }

        [HttpGet]
        [CheckMenu]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OperationLog(LogType.select)]
        public ContentResult List([FromForm] PubParams.InventoryBootstrapParams bootstrap)
        {
            var sd = _inventoryrecordServices.PageList(bootstrap);
            return Content(sd);
        }
    }
}