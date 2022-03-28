using FluentValidation.Results;
using IServices;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq;
using M.Core.Dto;
using M.Core.Entity;
using M.Core.Entity.Fluent.Validation;
using M.NetCore.Attributes;
using M.NetCore.NetCoreApp;
using M.Utils.Extensions;
using M.Utils.Pub;
using M.Utils.Table;

namespace MountainWMS.Controllers
{
    public class RoleController : BaseController
    {
        private readonly ISys_menuServices _menuServices;
        private readonly ISys_roleServices _roleServices;
        private readonly ISys_rolemenuServices _rolemenuServices;
        private readonly ISys_rolemenuauthServices _rolemenuauthServices;
        private readonly ISys_userServices _userServices;

        public RoleController(
            ISys_userServices userServices,
            ISys_rolemenuServices rolemenuServices,
            ISys_rolemenuauthServices rolemenuauthServices,
            ISys_roleServices roleServices,
            ISys_menuServices menuServices)
        {
            _userServices = userServices;
            _menuServices = menuServices;
            _roleServices = roleServices;
            _rolemenuServices = rolemenuServices;
            _rolemenuauthServices = rolemenuauthServices;
        }

        [CheckMenu]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OperationLog(LogType.select)]
        public ContentResult List([FromForm] Bootstrap.BootstrapParams bootstrap)
        {
            var sd = _roleServices.PageList(bootstrap);
            return Content(sd);
        }

        /// <summary>
        /// 获取全部菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMenuList()
        {
            return BootJsonH(_roleServices.GetMenu());
        }

        /// <summary>
        /// 获取当前角色分配的菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRoleMenuList(string id)
        {
            return BootJsonH(_roleServices.GetRoleMenu(SqlFunc.ToInt64(id)));
        }

        [HttpGet]
        public IActionResult Add(string id)
        {
            var roles = new RoleMenuDto();
            if (id.IsEmpty())
            {
                return View(roles);
            }
            else
            {
                var role = _roleServices.QueryableToEntity(c => c.RoleId == SqlFunc.ToInt64(id));
                var list = _rolemenuServices.QueryableToList(c => c.RoleId == SqlFunc.ToInt64(id));
                roles = new RoleMenuDto()
                {
                    RoleId = role?.RoleId.ToString(),
                    RoleName = role?.RoleName,
                    RoleType = role?.RoleType,
                    Remark = role?.Remark,
                    Children = list
                };
                return View(roles);
            }
        }

        [HttpGet]
        public IActionResult Query(string id)
        {
            var roles = new RoleMenuDto();
            if (id.IsEmpty())
            {
                return View(roles);
            }
            else
            {
                var role = _roleServices.QueryableToEntity(c => c.RoleId == SqlFunc.ToInt64(id));
                var list = _rolemenuServices.QueryableToList(c => c.RoleId == SqlFunc.ToInt64(id));
                roles = new RoleMenuDto()
                {
                    RoleId = role?.RoleId.ToString(),
                    RoleName = role?.RoleName,
                    RoleType = role?.RoleType,
                    Remark = role?.Remark,
                    Children = list
                };
                return View(roles);
            }
        }
        [HttpGet]
        public IActionResult Auth(string id)
        {
            var roles = new RoleMenuAuthDto();
            if (id.IsEmpty())
            {
                return View(roles);
            }
            else
            {
                var role = _roleServices.QueryableToEntity(c => c.RoleId == SqlFunc.ToInt64(id));
                var list = _rolemenuauthServices.QueryableToList(c => c.RoleId == SqlFunc.ToInt64(id));
                roles = new RoleMenuAuthDto()
                {
                    RoleId = role?.RoleId.ToString(),
                    Children = list
                };
                return View(roles);
            }
        }
        [HttpPost]
        [OperationLog(LogType.update)]
        public IActionResult UpdateAuth([FromForm] Sys_role role, [FromForm] string id, [FromForm] string[] menuAuthId)
        {
            if (id.IsEmptyZero())
            {
                return BootJsonH((false, PubConst.Update2));
            }
            else
            {
                role.RoleId = id.ToInt64();
                var flag = _roleServices.UpdateAuth(role, UserDtoCache.UserId, menuAuthId);
                return BootJsonH(flag.IsSuccess ? (flag.IsSuccess, PubConst.Update1) : (flag.IsSuccess, PubConst.Update2));
            }
        }

        [HttpPost]
        [OperationLog(LogType.addOrUpdate)]
        public IActionResult AddOrUpdate([FromForm] Sys_role role, [FromForm] string id, [FromForm] string[] menuId)
        {
            //int[] menuIds = Array.ConvertAll(menuId, new Converter<string, int>(c => c.ToInt32()));
            //Array.Sort(menuIds);
            //var arrs = (from d in menuId orderby d.ToInt32() ascending select d.ToInt32()).ToArray();
            //var arrs2 = (from d in arrs select d).ToArray();

            var validator = new SysRoleFluent();
            ValidationResult results = validator.Validate(role);
            bool success = results.IsValid;
            if (!success)
            {
                string msg = results.Errors.Aggregate("", (current, item) => (current + item.ErrorMessage + "</br>"));
                return BootJsonH((PubEnum.Failed.ToInt32(), msg));
            }
            if (id.IsEmptyZero())
            {
                if (_roleServices.IsAny(c => c.RoleName == role.RoleName))
                {
                    return BootJsonH((false, PubConst.Role2));
                }
                var flag = _roleServices.Insert(role, UserDtoCache.UserId, menuId);
                return BootJsonH(flag.IsSuccess ? (flag.IsSuccess, PubConst.Add1) : (flag.IsSuccess, PubConst.Add2));
            }
            else
            {
                //admin
                //var model = _roleServices.QueryableToEntity(c => c.RoleId == SqlFunc.ToInt64(id));
                //if (model.RoleType == "admin")
                //{
                //    return BootJsonH((false, PubConst.Role1));
                //}
                role.RoleId = id.ToInt64();
                var flag = _roleServices.Update(role, UserDtoCache.UserId, menuId);
                return BootJsonH(flag.IsSuccess ? (flag.IsSuccess, PubConst.Update1) : (flag.IsSuccess, PubConst.Update2));
            }
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult Delete(string id)
        {
            var model = _roleServices.QueryableToEntity(c => c.RoleId == SqlFunc.ToInt64(id));
            if (model.RoleType == "admin")
            {
                return BootJsonH((false, PubConst.Role1));
            }
            var user = _userServices.IsAny(c => c.RoleId == SqlFunc.ToInt64(id));
            if (user)
            {
                return BootJsonH((false, PubConst.Role3));
            }
            var flag = _roleServices.Update(new Sys_role { RoleId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            if (flag)
            {
                _rolemenuServices.Delete(c => c.RoleId == SqlFunc.ToInt64(id));
                return BootJsonH((flag, PubConst.Delete1));
            }
            else
            {
                return BootJsonH((flag, PubConst.Delete2));
            }
        }
    }
}