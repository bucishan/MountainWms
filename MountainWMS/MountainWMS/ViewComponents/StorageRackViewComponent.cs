using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using M.Utils.Extensions;
using M.Utils.Pub;
using IServices;

namespace MountainWMS.ViewComponents
{
    public class StorageRackViewComponent : ViewComponent
    {
        private readonly IWms_storagerackServices _storagerackServices;

        public StorageRackViewComponent(IWms_storagerackServices storagerackServices)
        {
            _storagerackServices = storagerackServices;
        }

        public async Task<IViewComponentResult> InvokeAsync(string model , string style = "",string title = "请选择")
        {
            ViewData["title"] = title;
            ViewData["field"] = model;
            ViewData["style"] = style;
            var list = await _storagerackServices.QueryableToList(c => c.IsDel == 1).ToListAsync();
            return View(list);
        }
    }
}