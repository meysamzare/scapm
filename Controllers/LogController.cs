using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;

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

        public LogController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IHttpContextAccessor _accessor)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            accessor = _accessor;
        }


        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> setLog([FromBody] setLogParam setLogParam)
        {
            try
            {
                SystemLog log = new SystemLog
                {
                    Date = DateTime.Now,
                    agentId = setLogParam.agentId,
                    agentName = setLogParam.agentName,
                    agnetType = setLogParam.agentType,
                    Desc = setLogParam.desc,
                    Event = setLogParam.ev,
                    Ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    LogSource = setLogParam.logSource
                };

                await db.SystemLogs.AddAsync(log);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] GetLogParam param)
        {
            try
            {
                var getparams = param.getparam;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.SystemLogs.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Event.Contains(query) || c.Desc.Contains(query) || c.Ip.Contains(query) || c.agentName.Contains(query) ||
                                    c.agnetType.Contains(query) || c.agentId.ToString().Contains(query) || c.LogSource.Contains(query));
                }

                if (!string.IsNullOrEmpty(param.selectedEvent))
                {
                    sl = sl.Where(c => c.Event == param.selectedEvent);
                }
                if (!string.IsNullOrEmpty(param.selectedAgentType))
                {
                    sl = sl.Where(c => c.agnetType == param.selectedAgentType);
                }
                if (!string.IsNullOrEmpty(param.selectedLogSource))
                {
                    sl = sl.Where(c => c.LogSource == param.selectedLogSource);
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("event"))
                    {
                        sl = sl.OrderBy(c => c.Event);
                    }
                    if (getparams.sort.Equals("logSource"))
                    {
                        sl = sl.OrderBy(c => c.LogSource);
                    }
                    if (getparams.sort.Equals("agentType"))
                    {
                        sl = sl.OrderBy(c => c.agnetType);
                    }
                    if (getparams.sort.Equals("agent"))
                    {
                        sl = sl.OrderBy(c => c.agentName).ThenBy(c => c.agentId);
                    }
                    if (getparams.sort.Equals("ip"))
                    {
                        sl = sl.OrderBy(c => c.Ip);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.Date);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("event"))
                    {
                        sl = sl.OrderByDescending(c => c.Event);
                    }
                    if (getparams.sort.Equals("logSource"))
                    {
                        sl = sl.OrderByDescending(c => c.LogSource);
                    }
                    if (getparams.sort.Equals("agentType"))
                    {
                        sl = sl.OrderByDescending(c => c.agnetType);
                    }
                    if (getparams.sort.Equals("agent"))
                    {
                        sl = sl.OrderByDescending(c => c.agentName).ThenByDescending(c => c.agentId);
                    }
                    if (getparams.sort.Equals("ip"))
                    {
                        sl = sl.OrderByDescending(c => c.Ip);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.Date);
                    }
                }
                else
                {
                    sl = sl.OrderByDescending(c => c.Date);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new
                    {
                        Id = c.Id,
                        dateString = c.Date.ToPersianDateWithTime(),
                        agentId = c.agentId,
                        agentName = c.agentName,
                        agnetType = c.agnetType,
                        Event = c.Event,
                        Ip = c.Ip,
                        LogSource = c.LogSource
                    })
                .ToListAsync();

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
        public async Task<IActionResult> getDesc([FromBody] Guid id)
        {
            try
            {
                var log = await db.SystemLogs.FirstOrDefaultAsync(c => c.Id == id);

                return this.DataFunction(true, log.Desc);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        
    }

    public class GetLogParam
    {
        public getparams getparam { get; set; }

        public string selectedEvent { get; set; }
        public string selectedLogSource { get; set; }
        public string selectedAgentType { get; set; }
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
}