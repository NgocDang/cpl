using CPL.Misc;
using CPL.Misc.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Configuration;

namespace CPL.Controllers
{
    public class MobileAppSettingsController : Controller
    {
        [HttpGet]
        [Permission(EnumRole.Guest)]
        public IActionResult GetAppSettings()
        {
            try
            {
                return new JsonResult(
                    new {
                        code = EnumResponseStatus.SUCCESS,
                        server_address = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
                        server_api_address = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}",
                        app_copyright = ConfigurationManager.AppSettings["MobileAppSettings:Copyright"],
                        app_title = ConfigurationManager.AppSettings["MobileAppSettings:AppTitle"],
                        app_version = ConfigurationManager.AppSettings["MobileAppSettings:Version"],
                        company_name = ConfigurationManager.AppSettings["MobileAppSettings:CompanyName"],
                        contact_email = ConfigurationManager.AppSettings["MobileAppSettings:ContactEmail"],
                        contact_phone = ConfigurationManager.AppSettings["MobileAppSettings:ContactPhone"],
                        system_maintenance = ConfigurationManager.AppSettings["MobileAppSettings:SystemMaintenance"]
                    }
                );
            }
            catch(Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.ERROR,
                        error_message_key = ex.Message
                    }
                );
            }
        }
    }
}