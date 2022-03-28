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
    public class AuthController : BaseController
    {
        private readonly ISys_menuServices _menuServices;
        private readonly ISys_authServices _authServices;
        private readonly ISys_rolemenuServices _rolemenuServices;
        private readonly ISys_userServices _userServices;

        public AuthController(ISys_userServices userServices, ISys_rolemenuServices rolemenuServices, ISys_authServices authServices, ISys_menuServices menuServices)
        {
            _userServices = userServices;
            _menuServices = menuServices;
            _authServices = authServices;
            _rolemenuServices = rolemenuServices;
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
            var sd = _authServices.PageList(bootstrap);
            return Content(sd);
        }

        [HttpGet]
        public IActionResult GetAuth()
        {
            var query = _authServices.QueryableToList(c => c.IsDel == 1).Select(c => new
            {
                Id = c.AuthId.ToString(),
                Name = c.AuthName,
            });
            return BootJsonH(query);
        }

        public IActionResult Add(string id)
        {
            var auth = new Sys_auth();
            if (id.IsEmpty())
            {
                return View(auth);
            }
            else
            {
                auth = _authServices.QueryableToEntity(c => c.AuthId == SqlFunc.ToInt64(id));
                return View(auth);
            }
        }
        [HttpPost]
        [OperationLog(LogType.addOrUpdate)]
        public IActionResult AddOrUpdate([FromForm] Sys_auth auth, [FromForm] string id)
        {
            var validator = new SysAuthFluent();
            ValidationResult results = validator.Validate(auth);
            bool success = results.IsValid;
            if (!success)
            {
                string msg = results.Errors.Aggregate("", (current, item) => (current + item.ErrorMessage + "</br>"));
                return BootJsonH((PubEnum.Failed.ToInt32(), msg));
            }
            if (id.IsEmptyZero())
            {
                if (_authServices.IsAny(c => c.AuthName == auth.AuthName))
                {
                    return BootJsonH((false, PubConst.Auth2));
                }
                auth.AuthId = PubId.SnowflakeId;
                auth.CreateBy = UserDtoCache.UserId;
                bool flag = _authServices.Insert(auth);
                return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                auth.AuthId = id.ToInt64();
                auth.ModifiedBy = UserDtoCache.UserId;
                auth.ModifiedDate = DateTimeExt.DateTime;
                bool flag = _authServices.Update(auth);
                return BootJsonH(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult Delete(string id)
        {
            var model = _authServices.QueryableToEntity(c => c.AuthId == SqlFunc.ToInt64(id));
            if (model.Status == 1)
            {
                return BootJsonH((false, PubConst.Auth1));
            }
            //var user = _userServices.IsAny(c => c.RoleId == SqlFunc.ToInt64(id));
            //if (user)
            //{
            //    return BootJsonH((false, PubConst.Auth3));
            //}
            var flag = _authServices.Update(new Sys_auth { AuthId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }
    }
}