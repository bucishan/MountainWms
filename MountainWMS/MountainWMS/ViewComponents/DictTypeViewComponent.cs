using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using M.Utils.Extensions;
using M.Utils.Pub;

namespace MountainWMS.ViewComponents
{
    public class DictTypeViewComponent : ViewComponent
    {
        public DictTypeViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string model , string style = "",string title = "请选择")
        {
            ViewData["title"] = title;
            ViewData["field"] = model;
            ViewData["style"] = style; 
            var list = await EnumExt.ToKVListLinq<PubDictType>().ToAsync();
            return View(list);
        }
    }
}