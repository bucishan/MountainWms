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
    public class MenuController : BaseController
    {
        private readonly ISys_menuServices _menuServices;
        private readonly ISys_menuauthServices _menuauthServices;
        

        public MenuController(ISys_menuServices menuServices, ISys_menuauthServices menuauthServices)
        {
            _menuServices = menuServices;
            _menuauthServices = menuauthServices;
        }

        [CheckMenu]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult GetMenuAuth(string id)
        //{
        //    var query = _menuauthServices.QueryableToEntity<Sys_menuauth>
        //    return BootJsonH(_roleServices.GetMenu());
        //}

        /// <summary>
        /// 主表
        /// </summary>
        /// <param name="bootstrap">参数</param>
        /// <returns></returns>
        [HttpPost]
        [OperationLog(LogType.select)]
        public ContentResult List([FromForm] PubParams.StockOutBootstrapParams bootstrap)
        {
            var sd = _menuServices.PageList(bootstrap);
            return Content(sd);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpPost]
        [OperationLog(LogType.select)]
        public ContentResult ListDetail(string pid)
        {
            var sd = _menuServices.PageListDetail(pid);
            return Content(sd);
        }

        public IActionResult Add(string id)
        {
            var menu = new Sys_menu();
            if (id.IsEmpty())
            {
                return View(menu);
            }
            else
            {
                menu = _menuServices.QueryableToEntity(c => c.MenuId == SqlFunc.ToInt64(id));
                return View(menu);
            }
        }
        [HttpGet]
        public IActionResult Detail(string id, string pid)
        {
            var model = new MenuAuthDto();
            if (id.IsEmpty())
            {
                model.MenuParent = pid.ToInt64();
                return View(model);
            }
            else
            {
                model = _menuServices.GetMenuSelectAuthList(id.ToInt64());
                return View(model);
            }
        }

        [HttpPost]
        [FilterXss]
        [OperationLog(LogType.addOrUpdate)]
        public IActionResult AddOrUpdate([FromForm] Sys_menu sys_Menu, [FromForm] string id)
        {
            var validator = new SysMenuFluent();
            ValidationResult results = validator.Validate(sys_Menu);
            bool success = results.IsValid;
            if (!success)
            {
                string msg = results.Errors.Aggregate("", (current, item) => (current + item.ErrorMessage + "</br>"));
                return BootJsonH((PubEnum.Failed.ToInt32(), msg));
            }
            if (id.IsEmptyZero())
            {
                if (_menuServices.IsAny(c => c.MenuName == sys_Menu.MenuName))
                {
                    return BootJsonH((false, PubConst.Menu1));
                }
                sys_Menu.MenuId = PubId.SnowflakeId;
                sys_Menu.MenuUrl = "#";
                sys_Menu.MenuParent = -1;
                sys_Menu.MenuType = "menu";
                sys_Menu.CreateBy = UserDtoCache.UserId;
                bool flag = _menuServices.Insert(sys_Menu);
                return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                sys_Menu.MenuId = id.ToInt64();
                sys_Menu.ModifiedBy = UserDtoCache.UserId;
                sys_Menu.ModifiedDate = DateTimeExt.DateTime;
                bool flag = _menuServices.Update(sys_Menu);
                return BootJsonH(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult Delete(string id)
        {
            var flag = _menuServices.Update(new Sys_menu { MenuId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult DeleteDetail(string id)
        {
            var flag = _menuServices.Update(new Sys_menu { MenuId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            if (flag)
            {
                _menuauthServices.Delete(c => c.MenuId == SqlFunc.ToInt64(id));
                return BootJsonH((flag, PubConst.Delete1));
            }
            else
            {
                return BootJsonH((flag, PubConst.Delete2));
            }
        }

        [HttpGet]
        [OperationLog(LogType.update)]
        public IActionResult Enable(string id, string type)
        {
            if (type == "1")
            {
                //禁用
                var flag = _menuServices.Update(new Sys_menu { MenuId = SqlFunc.ToInt64(id), Status = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.Status, c.ModifiedBy, c.ModifiedDate });
                return BootJsonH(flag ? (flag, PubConst.Enable3) : (flag, PubConst.Enable4));
            }
            else
            {
                //启用
                var flag = _menuServices.Update(new Sys_menu { MenuId = SqlFunc.ToInt64(id), Status = 1, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.Status, c.ModifiedBy, c.ModifiedDate });
                return BootJsonH(flag ? (flag, PubConst.Enable1) : (flag, PubConst.Enable2));
            }
        }

        [HttpPost]
        [OperationLog(LogType.addOrUpdate)]
        public IActionResult AddOrUpdateDetail([FromForm] Sys_menu menu, [FromForm] string id, [FromForm] string[] authId)
        {
            var validator = new SysMenuFluent();
            ValidationResult results = validator.Validate(menu);
            bool success = results.IsValid;
            if (!success)
            {
                string msg = results.Errors.Aggregate("", (current, item) => (current + item.ErrorMessage + "</br>"));
                return BootJsonH((PubEnum.Failed.ToInt32(), msg));
            }
            if (id.IsEmptyZero())
            {
                if (_menuServices.IsAny(c => c.MenuName == menu.MenuName))
                {
                    return BootJsonH((false, PubConst.Menu1));
                }
                var flag = _menuServices.InsertDetail(menu, UserDtoCache.UserId, authId);
                return BootJsonH(flag.IsSuccess ? (flag.IsSuccess, PubConst.Add1) : (flag.IsSuccess, PubConst.Add2));
            }
            else
            {
                menu.MenuId = id.ToInt64();
                var flag = _menuServices.UpdateDetail(menu, UserDtoCache.UserId, authId);
                return BootJsonH(flag.IsSuccess ? (flag.IsSuccess, PubConst.Update1) : (flag.IsSuccess, PubConst.Update2));
            }
        }
    }
}
