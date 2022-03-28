using IServices;
using M.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MountainWMS.ViewComponents
{
    public class NavViewComponent : ViewComponent
    {
        private readonly ISys_menuServices _menuServices;
        private readonly IHttpContextAccessor _httpContext;

        //private readonly ISys_authServices _authServices;

        public NavViewComponent(ISys_menuServices menuServices, IHttpContextAccessor httpContext)
        {
            _menuServices = menuServices;
            _httpContext = httpContext;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var requestPath = _httpContext.HttpContext.Request.Path.Value;
            var model = await GetItemsAsync(requestPath);
            return View(model);
        }

        private Task<MenuInfoDto> GetItemsAsync(string menuPath)
        {
            Task<MenuInfoDto> t1 = Task.Factory.StartNew(() => _menuServices.GetMenuInfo(menuPath));
            return t1;
        }
    }
}