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
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace SCMR_Api
{

    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }

    public static class FileHelpers
    {
        // If you require a check on specific characters in the IsValidFileExtensionAndSignature
        // method, supply the characters in the _allowedChars field.
        private static readonly byte[] _allowedChars = { };
        // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        // and the official specifications for the file types you wish to add.
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".zip", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                }
            },
        };

        // **WARNING!**
        // In the following file processing methods, the file's content isn't scanned.
        // In most production scenarios, an anti-virus/anti-malware scanner API is
        // used on the file before making the file available to users or other
        // systems. For more information, see the topic that accompanies this sample
        // app.

        public static async Task<byte[]> ProcessFormFile<T>(IFormFile formFile,
            ModelStateDictionary modelState, string[] permittedExtensions,
            long sizeLimit)
        {
            var fieldDisplayName = string.Empty;

            // Use reflection to obtain the display name for the model
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.
            MemberInfo property =
                typeof(T).GetProperty(
                    formFile.Name.Substring(formFile.Name.IndexOf(".",
                    StringComparison.Ordinal) + 1));

            if (property != null)
            {
                if (property.GetCustomAttribute(typeof(DisplayAttribute)) is
                    DisplayAttribute displayAttribute)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            // Don't trust the file name sent by the client. To display
            // the file name, HTML-encode the value.
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                formFile.FileName);

            // Check the file length. This check doesn't catch files that only have 
            // a BOM as their content.
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");

                return new byte[0];
            }

            if (formFile.Length > sizeLimit)
            {
                var megabyteSizeLimit = sizeLimit / 1048576;
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) exceeds " +
                    $"{megabyteSizeLimit:N1} MB.");

                return new byte[0];
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    // Check the content length in case the file's only
                    // content was a BOM and the content is actually
                    // empty after removing the BOM.
                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
                    }

                    if (!IsValidFileExtensionAndSignature(
                        formFile.FileName, memoryStream, permittedExtensions))
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) file " +
                            "type isn't permitted or the file's signature " +
                            "doesn't match the file's extension.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. " +
                    $"Please contact the Help Desk for support. Error: {ex.HResult}");
                // Log the exception
            }

            return new byte[0];
        }

        public static async Task<byte[]> ProcessStreamedFile(
            MultipartSection section, ContentDispositionHeaderValue contentDisposition,
            ModelStateDictionary modelState, string[] permittedExtensions, long sizeLimit)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(memoryStream);

                    // Check if the file is empty or exceeds the size limit.
                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError("File", "The file is empty.");
                    }
                    else if ((memoryStream.Length / 1048576) > sizeLimit)
                    {
                        var megabyteSizeLimit = sizeLimit / 1048576;
                        modelState.AddModelError("File",
                        $"The file exceeds {megabyteSizeLimit:N1} MB.");
                    }
                    // else if (!IsValidFileExtensionAndSignature(
                    //     contentDisposition.FileName.Value, memoryStream,
                    //     permittedExtensions))
                    // {
                    //     modelState.AddModelError("File",
                    //         "The file type isn't permitted or the file's " +
                    //         "signature doesn't match the file's extension.");
                    // }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError("File",
                    "The upload failed. Please contact the Help Desk " +
                    $" for support. Error: {ex.HResult}");
                // Log the exception
            }

            return new byte[0];
        }

        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    if (_allowedChars.Length == 0)
                    {
                        // Limits characters to ASCII encoding.
                        for (var i = 0; i < data.Length; i++)
                        {
                            if (reader.ReadByte() > sbyte.MaxValue)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Limits characters to ASCII encoding and
                        // values of the _allowedChars array.
                        for (var i = 0; i < data.Length; i++)
                        {
                            var b = reader.ReadByte();
                            if (b > sbyte.MaxValue ||
                                !_allowedChars.Contains(b))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                // Uncomment the following code block if you must permit
                // files whose signature isn't provided in the _fileSignature
                // dictionary. We recommend that you add file signatures
                // for files (when possible) for all file types you intend
                // to allow on the system and perform the file signature
                // check.
                /*
                if (!_fileSignature.ContainsKey(ext))
                {
                    return true;
                }
                */

                // File signature check
                // --------------------
                // With the file signatures provided in the _fileSignature
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }


    public static class Utility
    {

        public static string HtmlToPlaneText(this string htmlString, string htmlPlaceHolder = " ")
        {
            const string pattern = @"<.*?>";
            string sOut = Regex.Replace(htmlString, pattern, htmlPlaceHolder, RegexOptions.Singleline);
            sOut = sOut.Replace("&nbsp;", String.Empty);
            sOut = sOut.Replace("&amp;", "&");
            sOut = sOut.Replace("&gt;", ">");
            sOut = sOut.Replace("&lt;", "<");
            sOut = sOut.Replace("&zwnj;", String.Empty);
            return sOut;
        }

        public static string FormatNumberEvery3digit(this int number)
        {
            return String.Format("{0:#,###,###.##}", number);
        }

        public static string getFixed3Number(this object number)
        {
            return String.Format("{0:F3}", number);
        }

        public static string getFixed2Number(this object number)
        {
            return String.Format("{0:F2}", number);
        }


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
                    Type = "System Crash Report",
                    Object = e.ToString(),
                    Date = DateTime.Now,
                    Event = "System Crash Report",
                    ResponseData = " lineNumber = " + line + ", InnerException = " + e.InnerException,
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

        public static string PersianToEnglishDigit(this string persianStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                // Persian
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9',
                // Arabic
                ['٠'] = '0',
                ['١'] = '1',
                ['٢'] = '2',
                ['٣'] = '3',
                ['٤'] = '4',
                ['٥'] = '5',
                ['٦'] = '6',
                ['٧'] = '7',
                ['٨'] = '8',
                ['٩'] = '9'
            };
            foreach (var item in persianStr)
            {
                try
                {
                    persianStr = persianStr.Replace(item, LettersDictionary[item]);
                }
                catch { }
            }
            return persianStr;
        }

        public static bool EqualsAnyOf(this object value, params object[] targets)
        {
            return targets.Any(target => target != null ? target.Equals(value) : false);
        }

        public static string ToPersianDate(this DateTime date)
        {
            if (date < new DateTime(0622, 12, 30))
            {
                return "";
            }

            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("0#"), pc.GetDayOfMonth(date).ToString("0#"));
        }

        public static string ToPersianDate(this DateTime? date)
        {
            if (date.HasValue)
            {

                if (date < new DateTime(0622, 12, 30))
                {
                    return "";
                }

                PersianCalendar pc = new PersianCalendar();
                return string.Format("{0}/{1}/{2}", pc.GetYear(date.Value), pc.GetMonth(date.Value).ToString("0#"), pc.GetDayOfMonth(date.Value).ToString("0#"));
            }

            return "";
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

        public static string PersianDayString(this DateTime date)
        {
            if (date < new DateTime(0622, 12, 30))
            {
                return "";
            }

            var persianDateTime = new PersianDateTime(date);
            PersianCalendar pc = new PersianCalendar();

            return persianDateTime.ToString("dddd");
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
