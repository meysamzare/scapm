using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SCMR_Api.Model;

sealed class RoleAttribute : System.Attribute, IActionFilter
{
    readonly string[] roleTitles;
    readonly RolePrefix preFix;


    public RoleAttribute(RolePrefix preFix, params string[] roleTitles)
    {
        this.roleTitles = roleTitles;
        this.preFix = preFix;
    }

    public string[] roleProps
    {
        get
        {
            var returnDatas = new List<string>();

            roleTitles.ToList().ForEach(title =>
            {
                returnDatas.Add(Enum.GetName(preFix.GetType(), preFix) + "_" + title);
            });

            return returnDatas.ToArray();
        }
    }


    public void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<SCMR_Api.Data.DbContext>();

            var userName = context.HttpContext.User.Identity.Name;

            if (db.Users.Any(c => c.Username == userName))
            {
                var userRole = db.Users
                    .Include(c => c.Role)
                .First(c => c.Username == userName).Role;

                var accessList = new List<bool>();

                roleProps.ToList().ForEach(roleProp =>
                {
                    PropertyInfo pinfo = typeof(Role).GetProperty(roleProp);
                    var access = (bool)pinfo.GetValue(userRole, null);

                    accessList.Add(access);
                });


                if (accessList.All(c => c == false))
                {
                    context.Result = new NotFoundObjectResult("User Dose not Access to Requested Section!");
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("User not found!");
            }
        }
        catch { }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }

}

enum RolePrefix
{
    Add,
    Edit,
    Remove,
    View
}