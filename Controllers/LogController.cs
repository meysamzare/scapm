using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MD.PersianDateTime.Core;
using Microsoft.Extensions.DependencyInjection;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LogController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;
        private IHttpContextAccessor accessor;
        private IConfiguration _config;
        private IServiceScopeFactory _logScope;

        public LogController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config, IHttpContextAccessor _accessor, IServiceScopeFactory logScope)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            accessor = _accessor;
            _config = config;
            _logScope = logScope;
        }

        private static Object lock_Obj = new Object();


        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> setLog([FromBody] ILog log)
        {
            try
            {
                var date = DateTime.Now;
                var path = _getLogPath(date);

                var settings = await getLogSettingsAsync();

                if (!settings.saveResponseData)
                {
                    log.ResponseData = "log Response Data is Off...";
                }

                lock (lock_Obj)
                {
                    using (var stream = GetLogFileStream(path))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            using (var writer = new StreamWriter(stream))
                            {
                                string json = reader.ReadToEnd();
                                var existsLogs = JsonConvert.DeserializeObject<List<ILog>>(json);

                                if (existsLogs == null)
                                {
                                    existsLogs = new List<ILog>();
                                }

                                long lastId = 0;

                                if (existsLogs.Count != 0)
                                {
                                    lastId = existsLogs.Last().Id;
                                }

                                log.Id = lastId + 1;

                                log.Ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                                log.Date = date;
                                log.dateString = date.ToPersianDateWithTime();

                                existsLogs.Add(log);

                                stream.SetLength(0);

                                writer.WriteLine(JsonConvert.SerializeObject(existsLogs));
                            }
                        }
                    }
                }
                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e, true);
            }
        }

        [HttpPost]
        public IActionResult Get([FromBody] GetLogParam param)
        {
            try
            {
                var getparams = param.getparam;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var logsWithDate = getAllLogs();

                if (param.dateStart.HasValue)
                {
                    logsWithDate = logsWithDate.Where(c => c.date >= param.dateStart.Value).ToList();
                }

                if (param.dateEnd.HasValue)
                {
                    logsWithDate = logsWithDate.Where(c => c.date <= param.dateEnd.Value).ToList();
                }

                var logs = logsWithDate.SelectMany(c => c.logs);

                if (!string.IsNullOrWhiteSpace(query))
                {
                    logs = logs.Where(c => c.Event.Contains(query) || c.Table.Contains(query) || c.Ip.Contains(query) || c.agentName.Contains(query) ||
                                    c.agentType.Contains(query) || c.agentId.ToString().Contains(query) || c.LogSource.Contains(query));
                }

                if (!string.IsNullOrEmpty(param.selectedEvent))
                {
                    logs = logs.Where(c => c.Type == param.selectedEvent);
                }
                if (!string.IsNullOrEmpty(param.selectedAgentType))
                {
                    logs = logs.Where(c => c.agentType == param.selectedAgentType);
                }
                if (!string.IsNullOrEmpty(param.selectedLogSource))
                {
                    logs = logs.Where(c => c.LogSource == param.selectedLogSource);
                }
                if (!string.IsNullOrEmpty(param.table))
                {
                    logs = logs.Where(c => c.Table == param.table);
                }
                if (!string.IsNullOrEmpty(param.searchId))
                {
                    logs = logs.Where(c =>
                    {
                        if (c.TableObjectIds == null)
                        {
                            return false;
                        }
                        else
                        {
                            return c.TableObjectIds.Select(l => l.ToString()).Contains(param.searchId);
                        }
                    });
                }

                count = logs.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        logs = logs.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("event"))
                    {
                        logs = logs.OrderBy(c => c.Type);
                    }
                    if (getparams.sort.Equals("logSource"))
                    {
                        logs = logs.OrderBy(c => c.LogSource);
                    }
                    if (getparams.sort.Equals("agentType"))
                    {
                        logs = logs.OrderBy(c => c.agentType);
                    }
                    if (getparams.sort.Equals("agent"))
                    {
                        logs = logs.OrderBy(c => c.agentName).ThenBy(c => c.agentId);
                    }
                    if (getparams.sort.Equals("ip"))
                    {
                        logs = logs.OrderBy(c => c.Ip);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        logs = logs.OrderBy(c => c.Date);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        logs = logs.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("event"))
                    {
                        logs = logs.OrderByDescending(c => c.Event);
                    }
                    if (getparams.sort.Equals("logSource"))
                    {
                        logs = logs.OrderByDescending(c => c.LogSource);
                    }
                    if (getparams.sort.Equals("agentType"))
                    {
                        logs = logs.OrderByDescending(c => c.agentType);
                    }
                    if (getparams.sort.Equals("agent"))
                    {
                        logs = logs.OrderByDescending(c => c.agentName).ThenByDescending(c => c.agentId);
                    }
                    if (getparams.sort.Equals("ip"))
                    {
                        logs = logs.OrderByDescending(c => c.Ip);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        logs = logs.OrderByDescending(c => c.Date);
                    }
                }
                else
                {
                    logs = logs.OrderByDescending(c => c.Date);
                }

                logs = logs.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                logs = logs.Take(getparams.pageSize);

                var q = logs
                    .Select(c => new ILog
                    {
                        Id = c.Id,
                        dateString = c.dateString,
                        Date = c.Date,
                        agentId = c.agentId,
                        agentName = c.agentName,
                        agentType = c.agentType,
                        Event = c.Type,
                        Table = c.Table,
                        Ip = c.Ip,
                        LogSource = c.LogSource
                    })
                .ToList();

                return Json(new jsondata
                {
                    success = true,
                    data = q,
                    type = "" + count
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getDesc([FromBody] getDescParam param)
        {
            try
            {
                var logs = await getLogsOnDate(param.date);

                var log = logs.FirstOrDefault(c => c.Id == param.id);

                var content =
                    "\n\n Type is: " + log.Type + " \n\n\n"
                    + "Event is: " + log.Event + " \n\n\n"
                    + "Related Ids: \n " + getObjectArreyString(log.TableObjectIds) + " \n\n\n"
                    + "Object is: \n " + log.Object + " \n\n\n"
                    + "Old Object was: \n " + log.OldObject + " \n\n\n"
                    + "Deleted Object is: \n " + log.DeleteObjects + " \n\n\n"
                    + "Response Data is: \n" + log.ResponseData;

                return this.DataFunction(true, content);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        private string getObjectArreyString(object[] obj)
        {
            if (obj == null)
            {
                return "";
            }

            return string.Join(" & ", obj.Select(c => c.ToString()).ToList());
        }


        #region logOprations

        private async Task<List<ILog>> _ReadLogAsync(string path)
        {
            return JsonConvert.DeserializeObject<List<ILog>>(await System.IO.File.ReadAllTextAsync(path));
        }

        private List<ILog> _ReadLog(string path)
        {
            return JsonConvert.DeserializeObject<List<ILog>>(System.IO.File.ReadAllText(path));
        }

        private FileStream GetLogFileStream(string path)
        {
            return System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        private string _getLogPath(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            var persianDateString = pc.GetYear(date).ToString() + pc.GetMonth(date).ToString("0#") + pc.GetDayOfMonth(date).ToString("0#");

            var logPath = Path.Combine(hostingEnvironment.ContentRootPath, _config["Paths:Logs"], "log_" + persianDateString + ".json");

            return logPath;
        }

        private async Task<ILogSetting> getLogSettingsAsync()
        {
            var logPath = Path.Combine(hostingEnvironment.ContentRootPath, _config["Paths:Logs"], "settings.json");

            return JsonConvert.DeserializeObject<ILogSetting>(await System.IO.File.ReadAllTextAsync(logPath));
        }

        private async Task<List<ILog>> getLogsOnDate(DateTime date)
        {
            var path = _getLogPath(date);

            var logs = await _ReadLogAsync(path);

            return logs;
        }

        private List<ILogWithDate> getAllLogs()
        {
            var logsPath = Path.Combine(hostingEnvironment.ContentRootPath, _config["Paths:Logs"]);

            var allLogsNameList = Directory.EnumerateFiles(logsPath, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName)
            .Where(c => c.StartsWith("log_")).ToList();

            var allLogsList = new List<ILogWithDate>();

            foreach (var path in allLogsNameList)
            {
                var logPath = Path.Combine(logsPath, path);

                var purePersianDate = int.Parse(path.Split('.')[0].Split('_')[1]);

                var dateTime = PersianDateTime.Parse(purePersianDate).ToDateTime();

                var logs = _ReadLog(logPath);

                allLogsList.Add(new ILogWithDate
                {
                    date = dateTime,
                    logs = logs
                });
            }
            return allLogsList;
        }

        #endregion

    }

    public class getDescParam
    {
        public DateTime date { get; set; }

        public long id { get; set; }
    }

    public class GetLogParam
    {
        public getparams getparam { get; set; }

        public string selectedEvent { get; set; }
        public string selectedLogSource { get; set; }
        public string selectedAgentType { get; set; }
        public DateTime? dateStart { get; set; }
        public DateTime? dateEnd { get; set; }
        public string searchId { get; set; }
        public string table { get; set; }
    }

    public class setLogParam
    {
        public int agentId { get; set; }
        public string agentType { get; set; }
        public string agentName { get; set; }
        public string desc { get; set; }
        public string ev { get; set; }
        public string logSource { get; set; }
    }

    public class ILogWithDate
    {
        public DateTime date { get; set; }
        public List<ILog> logs { get; set; }
    }

    public class ILogSetting
    {
        public bool saveResponseData { get; set; }
    }

    public class ILog
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string dateString { get; set; }
        public int agentId { get; set; }
        public string agentType { get; set; }
        public string agentName { get; set; }
        public object Object { get; set; }
        public object OldObject { get; set; }
        public object DeleteObjects { get; set; }
        public string Table { get; set; }
        public object[] TableObjectIds { get; set; }
        public object ResponseData { get; set; }
        public string LogSource { get; set; }
        public string Ip { get; set; }
        public string Desc { get; set; }
        public string Event { get; set; }
    }
}