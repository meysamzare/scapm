using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using MD.PersianDateTime.Core;
using System.Diagnostics;

namespace SCMR_Api
{
    public static class Utility
    {


        public static JsonResult SuccessFunction(this Controller c)
        {
            return c.Json(new jsondata
            {
                success = true
            });
        }


        public static JsonResult SuccessFunction(this Controller c, string message)
        {
            return c.Json(new jsondata
            {
                success = true,
                message = message
            });
        }

        public static JsonResult SuccessFunction(this Controller c, string redirect, object data = null)
        {
            return c.Json(new jsondata
            {
                success = true,
                redirect = redirect,
                data = data
            });
        }

        public static JsonResult SuccessFunction(this Controller c, object data)
        {
            return c.Json(new jsondata
            {
                success = true,
                data = data
            });
        }

        public static JsonResult CatchFunction(this Controller c, Exception e, bool success = false)
        {
            int line = 0;

            try
            {
                IConfigurationRoot Configuration;

                var bilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = bilder.Build();

                var option = new DbContextOptionsBuilder<Data.DbContext>();
                option.UseSqlServer(Configuration["ConnectionStrings:Base"]);

                var db = new Data.DbContext(option.Options);

                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                 line = frame.GetFileLineNumber();


                db.ILogSystems.Add(new ILogSystem
                {
                    Date = DateTime.Now,
                    Event = "System Crash Report",
                    Desc = e.ToString() + " lineNumber = " + line + ", InnerException = " + e.InnerException,
                    agentId = 0,
                    agentName = "---",
                    agentType = "Api",
                    Ip = "---",
                    LogSource = "Api"
                });

                db.SaveChanges();
            }
            catch (System.Exception)
            {

            }


            return c.Json(new jsondata
            {
                success = success,
                message = "برنامه با خطا مواجه شده است " + e.Message + " l:" + line,
                type = "error"
            });
        }

        public static async Task<int> getActiveYeareducationId(this Controller controller)
        {
            IConfigurationRoot Configuration;

            var bilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = bilder.Build();

            var option = new DbContextOptionsBuilder<Data.DbContext>();
            option.UseSqlServer(Configuration["ConnectionStrings:Base"]);

            var db = new Data.DbContext(option.Options);

            var setting = await db.Settings.FirstOrDefaultAsync(c => c.Key == "NowYeareducationId");

            return int.Parse(setting.Value);
        }

        public static int getActiveYeareducationIdNonAsync(this Controller controller)
        {
            IConfigurationRoot Configuration;

            var bilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = bilder.Build();

            var option = new DbContextOptionsBuilder<Data.DbContext>();
            option.UseSqlServer(Configuration["ConnectionStrings:Base"]);

            var db = new Data.DbContext(option.Options);

            var setting = db.Settings.FirstOrDefault(c => c.Key == "NowYeareducationId");

            return int.Parse(setting.Value);
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
             => self.Select((item, index) => (item, index));

        public static JsonResult UnSuccessFunction(this Controller c, string message, string type = "warning")
        {
            return c.Json(new jsondata
            {
                success = false,
                message = message,
                type = type
            });
        }

        public static JsonResult UnSuccessFunction(this Controller c)
        {
            return c.Json(new jsondata
            {
                success = false
            });
        }


        public static JsonResult UnSuccessFunction(this Controller c, string message, string redirect, string type = "error")
        {
            return c.Json(new jsondata
            {
                success = false,
                message = message,
                type = type,
                redirect = redirect
            });
        }



        public static JsonResult CustomFunction(this Controller c, bool success, string message, string type, string redirect, object data = null)
        {
            return c.Json(new jsondata
            {
                success = success,
                message = message,
                type = type,
                redirect = redirect,
                data = data
            });
        }


        public static JsonResult DataFunction(this Controller c, bool success, Object data)
        {
            return c.Json(new jsondata
            {
                success = success,
                data = data
            });
        }

        // public static async Task<bool> SignInAsync(this Microsoft.AspNetCore.Http.HttpContext httpContext, string authName, params Claim[] claimsp)
        // {
        //     try
        //     {

        //         var claims = new List<Claim>
        //         {
        //             new Claim(ClaimTypes.Name, authName)
        //         };

        //         foreach (var i in claimsp)
        //         {
        //             if (i != null)
        //             {
        //                 claims.Add(i);
        //             }
        //         }

        //         var claimsIdentity = new ClaimsIdentity(
        //             claims,
        //             CookieAuthenticationDefaults.AuthenticationScheme);


        //         await httpContext.Authentication.SignInAsync(
        //             CookieAuthenticationDefaults.AuthenticationScheme,
        //         new ClaimsPrincipal(claimsIdentity));

        //         return true;
        //     }
        //     catch
        //     {
        //         return false;
        //     }
        // }

        // public static async Task<bool> SignOutAsync(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        // {
        //     try
        //     {
        //         await httpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //         return true;
        //     }
        //     catch
        //     {
        //         return false;
        //     }
        // }


        public static string ToPersianDate(this DateTime date)
        {
            if (date < new DateTime(0622, 12, 30))
            {
                return "";
            }

            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("0#"), pc.GetDayOfMonth(date).ToString("0#"));
        }

        public static string ToPersianDateWithDayString(this DateTime date)
        {
            if (date < new DateTime(0622, 12, 30))
            {
                return "";
            }
            var persianDateTime = new PersianDateTime(date);
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{3} {0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("0#"), pc.GetDayOfMonth(date).ToString("0#"), persianDateTime.ToString("dddd"));
        }

        public static string ToPersianDateWithTime(this DateTime date)
        {
            if (date < new DateTime(0622, 12, 30))
            {
                return "";
            }
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{3}:{4} {0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("0#"),
                        pc.GetDayOfMonth(date).ToString("0#"), pc.GetHour(date).ToString("0#"), pc.GetMinute(date).ToString("0#"));
        }

        public static int getPersianDateMonthInt(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetMonth(date);
        }


        public static DateTime PersianStringDateToDateTime(this string date)
        {
            return PersianDateTime.Parse(date).ToDateTime();
        }

        public static string ToEnglishDate(this DateTime date)
        {
            return string.Format("{0}/{1}/{2}", date.Year, date.Month, date.Day);
        }

        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                // ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);
                ViewEngineResult viewResult = viewEngine.GetView("~/", viewName, false);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }

    public static class DbSetExtension
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T data) where T : class
        {
            var context = dbSet.GetContext();
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);

            var t = typeof(T);
            List<PropertyInfo> keyFields = new List<PropertyInfo>();

            foreach (var propt in t.GetProperties())
            {
                var keyAttr = ids.Contains(propt.Name);
                if (keyAttr)
                {
                    keyFields.Add(propt);
                }
            }
            if (keyFields.Count <= 0)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }
            var entities = dbSet.AsNoTracking().ToList();
            foreach (var keyField in keyFields)
            {
                var keyVal = keyField.GetValue(data);
                entities = entities.Where(p => p.GetType().GetProperty(keyField.Name).GetValue(p).Equals(keyVal)).ToList();
            }
            var dbVal = entities.FirstOrDefault();
            if (dbVal != null)
            {
                context.Entry(dbVal).CurrentValues.SetValues(data);
                context.Entry(dbVal).State = EntityState.Modified;
                return;
            }
            dbSet.Add(data);
        }

        public static void AddOrUpdate<T>(this DbSet<T> dbSet, Expression<Func<T, object>> key, T data) where T : class
        {
            var context = dbSet.GetContext();
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);
            var t = typeof(T);
            var keyObject = key.Compile()(data);
            PropertyInfo[] keyFields = keyObject.GetType().GetProperties().Select(p => t.GetProperty(p.Name)).ToArray();
            if (keyFields == null)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }
            var keyVals = keyFields.Select(p => p.GetValue(data));
            var entities = dbSet.AsNoTracking().ToList();
            int i = 0;
            foreach (var keyVal in keyVals)
            {
                entities = entities.Where(p => p.GetType().GetProperty(keyFields[i].Name).GetValue(p).Equals(keyVal)).ToList();
                i++;
            }
            if (entities.Any())
            {
                var dbVal = entities.FirstOrDefault();
                var keyAttrs =
                    data.GetType().GetProperties().Where(p => ids.Contains(p.Name)).ToList();
                if (keyAttrs.Any())
                {
                    foreach (var keyAttr in keyAttrs)
                    {
                        keyAttr.SetValue(data,
                            dbVal.GetType()
                                .GetProperties()
                                .FirstOrDefault(p => p.Name == keyAttr.Name)
                                .GetValue(dbVal));
                    }
                    context.Entry(dbVal).CurrentValues.SetValues(data);
                    context.Entry(dbVal).State = EntityState.Modified;
                    return;
                }
            }
            dbSet.Add(data);
        }
    }

    public static class HackyDbSetGetContextTrick
    {
        public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            return (DbContext)dbSet
                .GetType().GetTypeInfo()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbSet);
        }
    }
}


public class jsondata
{
    public bool success { get; set; }

    public object data { get; set; }

    public string message { get; set; }

    public string type { get; set; }

    public string redirect { get; set; }

}
public class NotificationHubLifetimeManager<THub> : DefaultHubLifetimeManager<THub>
         where THub : Hub
{
    public NotificationHubLifetimeManager(ILogger<DefaultHubLifetimeManager<THub>> logger) : base(logger)
    {
    }
}
