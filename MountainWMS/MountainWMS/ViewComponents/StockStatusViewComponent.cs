using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using M.Utils.Extensions;
using M.Utils.Pub;
using System;

namespace MountainWMS.ViewComponents
{
    public class StockStatusViewComponent : ViewComponent
    {
        public StockStatusViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string field)
        {
            ViewData["field"] = field;
            var list = await EnumExt.ToKVListLinq<StockInStatus>().ToAsync();
            return View(list);
        }
    }
}