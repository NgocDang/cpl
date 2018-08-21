using CPL.Common.Enums;
using CPL.Core.Interfaces;
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
        public EnumEntity? Entity { get; set; }
        public EnumAction? Action { get; set; }
        public IAuthorizePermission AuthorizePermission { get; set; }

        public PermissionAttribute(EnumRole role)
        {
            this.Role = role;
        }

        public PermissionAttribute(EnumRole role, EnumEntity entity, EnumAction action)
        {
            this.Role = role;
            this.Entity = entity;
            this.Action = action;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if ((context.RouteData.Values["action"].ToString() == "Maintenance") && (context.RouteData.Values["controller"].ToString() == "Home"))
            {
                base.OnActionExecuting(context);
                return;
            }

            var remoteIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (CPLConstant.Maintenance.IsOnMaintenance && !CPLConstant.Maintenance.MaintenanceAllowedIp.Split(CPLConstant.Maintenance.MaintenanceAllowedIpDelimiter).Contains(remoteIp))
            {
                File.AppendAllText("log.txt", "Allowed ip: " + CPLConstant.Maintenance.MaintenanceAllowedIp + "Remote ip: " + remoteIp + Environment.NewLine);
                context.Result = new RedirectResult("/Home/Maintenance");
                return;
            }

            SetRole(Role);

            
            var isAuthenticated = AuthorizePermission.IsLoggedIn(context);
            if (isAuthenticated.Code == PermissionStatus.OkCode)
            {
                var isACL = AuthorizePermission.IsACL(context, Entity, Action);
                if (isACL.Code == PermissionStatus.OkCode)
                {
                    base.OnActionExecuting(context);
                }
                else
                {
                    if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest") // is Ajax request
                    {
                        context.Result = new RedirectResult(PermissionStatus.UnAuthorizedAjaxUrl);
                    }
                    else
                    {
                        context.Result = new RedirectResult(isACL.Url);
                    }
                    return;
                }
            }
            else
            {
                context.Result = new RedirectResult(isAuthenticated.Url);
                return;
            }
        }

        public void SetRole(EnumRole role)
        {
            this.Role = role;
            if (Role == EnumRole.Guest)
            {
                AuthorizePermission = new GuestAuthorizePermission();
            }
            else if (Role == EnumRole.Admin)
            {
                AuthorizePermission = new AdminAuthorizePermission();
            }
            else if (Role == EnumRole.User)
            {
                AuthorizePermission = new UserAuthorizePermission();
            }
        }
    }
}
