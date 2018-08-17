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

            var isAuthorized = AuthorizePermission.IsLoggedIn(context) && AuthorizePermission.IsACL(context, Entity, Action);




            //var user = context.HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            //if (Role == EnumRole.User && user == null)
            //    context.Result = new RedirectResult("/Authentication/LogIn");
            //else if (Role == EnumRole.Admin && user == null)
            //    context.Result = new RedirectResult("/Authentication/LogIn");
            //else if (Role == EnumRole.Admin && user != null && !user.IsAdmin)
            //    context.Result = new RedirectResult("/Error/Error403");
            //base.OnActionExecuting(context);
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

    public interface IAuthorizePermission
    {
        bool IsLoggedIn(ActionExecutingContext context);
        bool IsACL(ActionExecutingContext context, EnumEntity? entity, EnumAction? action);
    }

    public class BaseAuthorizePermission: IAuthorizePermission
    {
        public virtual bool IsACL(ActionExecutingContext context, EnumEntity? entity, EnumAction? action)
        {
            throw new NotImplementedException();
        }

        public bool IsLoggedIn(ActionExecutingContext context)
        {
            return context.HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser") != null;
        }
    }

    public class GuestAuthorizePermission : BaseAuthorizePermission, IAuthorizePermission
    {
        public override bool IsACL(ActionExecutingContext context, EnumEntity? entity, EnumAction? action)
        {
            return true;
        }
    }

    public class AdminAuthorizePermission : BaseAuthorizePermission, IAuthorizePermission
    {
        public override bool IsACL(ActionExecutingContext context, EnumEntity? entity, EnumAction? action)
        {



            return true;
        }
    }

    public class UserAuthorizePermission : BaseAuthorizePermission, IAuthorizePermission
    {
        public override bool IsACL(ActionExecutingContext context, EnumEntity? entity, EnumAction? action)
        {


            return false;
        }
    }


    public class TransactionAuthorize : IAuthorize
    {
        public ActionExecutingContext Context { get; set; }
        public EnumAction? Action { get; set; }

        public TransactionAuthorize(ActionExecutingContext context, EnumRole role, EnumAction? action)
        {
            this.Context = context;
            this.Action = action;
        }

        public bool IsACL()
        {
            var sysUserService = (ISysUserService)Context.HttpContext.RequestServices.GetService(typeof(ISysUserService));
            var transactionHistoryService = (ICoinTransactionService)Context.HttpContext.RequestServices.GetService(typeof(ICoinTransactionService));

            var currentUser = Context.HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            if (Context.RouteData.Values["controller"].ToString() == "History" && Context.RouteData.Values["action"].ToString() == "TransactionDetail")
            {
                var currentUserTransactionIdList = transactionHistoryService.Queryable().Where(x => x.SysUserId == currentUser.Id).Select(x => x.Id).ToList();
                var currentTransactionId = Context.RouteData.Values["id"].ToString();
                if (!currentUser.IsAdmin && !string.IsNullOrEmpty(currentTransactionId) && !currentUserTransactionIdList.Contains(int.Parse(currentTransactionId)))
                    return false;
            }
            else if (Context.RouteData.Values["controller"].ToString() == "History" && Context.RouteData.Values["action"].ToString() == "Transaction")
            {
                if (!string.IsNullOrEmpty(Context.HttpContext.Request.Query["sysUserId"]))
                {
                    if (!currentUser.IsAdmin && currentUser.Id != int.Parse(Context.HttpContext.Request.Query["sysUserId"]))
                        return false;
                }
            }
            return true;
        }
    }

    public class LotteryHistoryAuthorize : IAuthorize
    {
        public ActionExecutingContext Context { get; set; }
        public EnumAction? Action { get; set; }

        public LotteryHistoryAuthorize(ActionExecutingContext context, EnumAction? action)
        {
            this.Context = context;
            this.Action = action;
        }

        public bool IsAuthorize()
        {
            var sysUserService = (ISysUserService)Context.HttpContext.RequestServices.GetService(typeof(ISysUserService));
            var transactionHistoryService = (ICoinTransactionService)Context.HttpContext.RequestServices.GetService(typeof(ICoinTransactionService));

            var currentUser = Context.HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            if (!string.IsNullOrEmpty(Context.HttpContext.Request.Query["sysUserId"]) && !currentUser.IsAdmin && currentUser.Id != int.Parse(Context.HttpContext.Request.Query["sysUserId"].ToString()))
                return false;
            return true;
        }
    }

    public class SysUserAuthorize : IAuthorize
    {
        public ActionExecutingContext Context { get; set; }
        public EnumAction? Action { get; set; }

        public SysUserAuthorize(ActionExecutingContext context, EnumAction? action)
        {
            this.Context = context;
            this.Action = action;
        }

        public bool IsAuthorize()
        {
            var sysUserService = (ISysUserService)Context.HttpContext.RequestServices.GetService(typeof(ISysUserService));

            var currentUser = Context.HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");

            if (Action == EnumAction.Delete)
            {
                var beingDeletedUser = sysUserService.Queryable().FirstOrDefault(x => x.Id == int.Parse(Context.ActionArguments["id"].ToString()));
                if (beingDeletedUser == null || !currentUser.IsAdmin || beingDeletedUser.IsAdmin)
                    return false;
            }
            return true;
        }
    }
}
