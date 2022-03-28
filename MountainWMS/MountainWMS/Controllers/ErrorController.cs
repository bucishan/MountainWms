using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using M.Models;
using M.NetCore.NetCoreApp;

namespace MountainWMS.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController() {}

        [HttpGet("/Error")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Error/Error")]
        [HttpGet("/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int statusCode)
        {
            ErrorViewModel model = new ErrorViewModel();
            model.StatusCode = statusCode;
            switch (statusCode)
            {
                case 404:
                    model.ErrorMessage = "抱歉，读者访问的页面不存在";
                    break;
                case 405:
                    model.ErrorMessage = "抱歉，您的请求类型有错误";
                    break;
                default:
                    model.ErrorMessage = "抱歉，出现未知错误";
                    break;
            }
            return View(model);

        }
    }
}