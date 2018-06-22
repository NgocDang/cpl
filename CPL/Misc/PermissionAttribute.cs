using CPL.Common.Enums;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAttribute : ActionFilterAttribute
    {
        public EnumRole Role { get; set; }
        public PermissionAttribute(EnumRole role)
        {
            this.Role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if ((context.RouteData.Values["action"].ToString() == "Maintenance") && (context.RouteData.Values["controller"].ToString() == "Home"))
            {
                base.OnActionExecuting(context);
                return;
            }

            var user = context.HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (CPLConstant.Maintenance.IsOnMaintenance && !CPLConstant.Maintenance.MaintenanceAllowedIp.Split(CPLConstant.Maintenance.MaintenanceAllowedIpDelimiter).Contains(remoteIp))
            {
                File.AppendAllText("log.txt", "Allowed ip: " + CPLConstant.Maintenance.MaintenanceAllowedIp + "Remote ip: " + remoteIp + Environment.NewLine);
                context.Result = new RedirectResult("/Home/Maintenance");
            }
            else if (Role == EnumRole.User && user == null)
                context.Result = new RedirectResult("/Authentication/LogIn");
            else if (Role == EnumRole.Admin && user == null)
                context.Result = new RedirectResult("/Authentication/LogIn");
            else if (Role == EnumRole.Admin && user != null && !user.IsAdmin)
                context.Result = new RedirectResult("/Error/Error403");
            base.OnActionExecuting(context);
        }
    }
}
