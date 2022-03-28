using IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using M.Utils.Extensions;
using M.Core.Dto;
using Microsoft.AspNetCore.Http;
using M.Core.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MountainWMS.ViewComponents
{
    public class MenuAuthViewComponent : ViewComponent
    {
        private readonly ISys_rolemenuauthServices _rolemenuauthServices;
        private readonly ISys_menuauthServices _menuauthServices;
        private readonly IHttpContextAccessor _httpContext;

        //private readonly ISys_authServices _authServices;

        public MenuAuthViewComponent(
            ISys_rolemenuauthServices rolemenuauthServices,
            ISys_menuauthServices menuauthServices, 
            IHttpContextAccessor httpContext)
        {
            _rolemenuauthServices = rolemenuauthServices;
            _menuauthServices = menuauthServices;
            _httpContext = httpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var requestPath = _httpContext.HttpContext.Request.Path.Value;
            var model = await GetRoleItemsAsync(requestPath);
            return View(model);
        }

        /// <summary>
        /// 获取当前地址可用权限
        /// </summary>
        /// <param name="menuPath"></param>
        /// <returns></returns>
        private Task<List<Sys_auth>> GetItemsAsync(string menuPath)
        {
            Task<List<Sys_auth>> t1 = Task.Factory.StartNew(() => _menuauthServices.GetMenuSelectAuthList(menuPath));
            return t1;
        }
        /// <summary>
        /// 获取当前地址可 用户所在角色可用权限
        /// </summary>
        /// <param name="menuPath"></param>
        /// <returns></returns>
        private Task<List<Sys_auth>> GetRoleItemsAsync(string menuPath)
        {
            var claims = _httpContext.HttpContext.User?.Claims;
            long roleId = claims.SingleOrDefault(c => c.Type == ClaimTypes.Role).Value.ToInt64();
            Task<List<Sys_auth>> t1 = Task.Factory.StartNew(() => _rolemenuauthServices.GetMenuSelectAuthList(menuPath, roleId));
            return t1;
        }
    }
}