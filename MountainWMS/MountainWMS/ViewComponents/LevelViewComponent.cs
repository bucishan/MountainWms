using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using M.Utils.Extensions;
using M.Utils.Pub;

namespace MountainWMS.ViewComponents
{
    public class LevelViewComponent : ViewComponent
    {
        public LevelViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await EnumExt.ToKVListLinq<PubLevel>().ToAsync();
            return View(list);
        }
    }
}