using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SCMR_Api.Model;
using WebPush;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;
        private IConfiguration config;

        public NotificationController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration _config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            config = _config;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeParam param)
        {
            try
            {
                if (await db.NotificationAgents.AnyAsync(c => c.Endpoint == param.sub.Endpoint))
                {
                    return this.UnSuccessFunction("قبلا ثبت شده است");
                }

                if (await db.NotificationAgents.AnyAsync(c => c.StudentId == param.stdId && c.IsParent == param.isParent))
                {
                    var agent = await db.NotificationAgents.FirstOrDefaultAsync(c => c.StudentId == param.stdId && c.IsParent == param.isParent);
                    db.NotificationAgents.Remove(agent);
                }

                await db.NotificationAgents.AddAsync(new Model.NotificationAgent
                {
                    Auth = param.sub.Auth,
                    Endpoint = param.sub.Endpoint,
                    P256DH = param.sub.P256DH,
                    IsParent = param.isParent,
                    StudentId = param.stdId,
                    SubscribeDate = DateTime.Now
                });

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Unsubscribe([FromBody] string endPoint)
        {
            try
            {
                var agent = await db.NotificationAgents.FirstOrDefaultAsync(c => c.Endpoint == endPoint);

                if (agent != null)
                {
                    db.NotificationAgents.Remove(agent);

                    await db.SaveChangesAsync();
                }

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Broadcast([FromBody] BrodcastParam param)
        {
            try
            {
                var client = new WebPushClient();

                var notification = await db.Notifications.FirstOrDefaultAsync(c => c.Id == param.notificationId);

                var serializedNotification = JsonConvert.SerializeObject(notification);

                var vapid = new VapidDetails
                {
                    PrivateKey = config["VapidDetails:PrivateKey"],
                    PublicKey = config["VapidDetails:PublicKey"],
                    Subject = config["VapidDetails:Subject"]
                };

                var unSuccessCount = 0;

                foreach (var id in param.agentIds)
                {
                    var agent = await db.NotificationAgents.FirstOrDefaultAsync(c => c.Id == id);

                    try
                    {
                        await client.SendNotificationAsync(agent.getPushSubscription, serializedNotification, vapid);
                    }
                    catch
                    {
                        unSuccessCount += 1;
                    }
                }

                notification.SendDate = DateTime.Now;

                // var user = await db.Users.Include(c => c.Role).FirstOrDefaultAsync(c => c.Username == User.Identity.Name);

                // notification.SenderName = user.fullName;
                // notification.SenderRole = user.Role.Name;

                await db.SaveChangesAsync();

                if (unSuccessCount != 0)
                {
                    return this.SuccessFunction(message: "تلنگر " + unSuccessCount + " مورد از موارد انتخابی با خطا مواجه شد");
                }

                return this.SuccessFunction(message: "با موفقیت ارسال شد");
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public IActionResult getNotificationAgents([FromBody] getNotificationAgentsParam param)
        {
            try
            {
                var agents = db.NotificationAgents
                    .Select(c => new
                    {
                        Id = c.Id,
                        IsParent = c.IsParent,
                        studentFullNameAndType = (c.IsParent ? "اولیای " : "") + c.Student.Name + " " + c.Student.LastName,
                        isStudentContainsGradeId = c.isStudentContainsGradeId(param.selectedGrade, c.Student.StdClassMngs),
                        isStudentContainsClassId = c.isStudentContainsClassId(param.selectedClass, c.Student.StdClassMngs)
                    })
                .AsEnumerable();

                if (param.showType == 0)
                {
                    agents = agents.Where(c => !c.IsParent);
                }

                if (param.showType == 1)
                {
                    agents = agents.Where(c => c.IsParent);
                }

                if (param.selectedGrade.HasValue)
                {
                    agents = agents.Where(c => c.isStudentContainsGradeId);
                }

                if (param.selectedClass.HasValue)
                {
                    agents = agents.Where(c => c.isStudentContainsClassId);
                }

                return this.DataFunction(true, agents);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Notification notification)
        {
            try
            {
                notification.CreateDate = DateTime.Now;

                await db.Notifications.AddAsync(notification);

                await db.SaveChangesAsync();


                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Notification notification)
        {
            try
            {
                db.Update(notification);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getparams getparams)
        {
            try
            {
                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.Notifications.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Title.Contains(query) || c.ButtonTitle.Contains(query) || c.Content.Contains(query) ||
                                    c.ShortContent.Contains(query));
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("shortContent"))
                    {
                        sl = sl.OrderBy(c => c.ShortContent);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderBy(c => c.NotiifcationType);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.SendDate);
                    }
                    if (getparams.sort.Equals("dateCreate"))
                    {
                        sl = sl.OrderBy(c => c.CreateDate);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("shortContent"))
                    {
                        sl = sl.OrderByDescending(c => c.ShortContent);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderByDescending(c => c.NotiifcationType);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.SendDate);
                    }
                    if (getparams.sort.Equals("dateCreate"))
                    {
                        sl = sl.OrderByDescending(c => c.CreateDate);
                    }
                }
                else
                {
                    sl = sl.OrderByDescending(c => c.Id);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        Content = c.Content,
                        ButtonTitle = c.ButtonTitle,
                        IsShow = c.IsShow,
                        NotiifcationType = c.NotiifcationType,
                        ShowType = c.ShowType,
                        sendDateString = c.SendDate.ToPersianDate(),
                        createDateString = c.CreateDate.ToPersianDate(),
                        notificationSeenCount = c.NotificationSeens.Count
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
        [AllowAnonymous]
        public async Task<IActionResult> getLoginNotification([FromBody] int type)
        {
            try
            {
                var notifications = await db.Notifications
                .Where(c => c.IsShow == true &&
                        c.ShowType == NotificationShowType.AllLogin || c.ShowType == (NotificationShowType)type)
                .ToListAsync();

                return this.DataFunction(true, notifications);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAll()
        {
            try
            {

                var sl = await db.Notifications
                    .Where(c => c.IsShow == true)
                .Select(c => new { id = c.Id, title = c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getNotification([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Notifications.FirstOrDefaultAsync(c => c.Id == id);

                    return this.DataFunction(true, sl);
                }
                else
                {
                    return this.UnSuccessFunction("Undefined Value", "error");
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var sl = await db.Notifications.FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.Notifications.Remove(sl);
                    }
                    else
                    {
                        return this.UnSuccessFunction("Undefined Value", "error");
                    }
                }

                await db.SaveChangesAsync();


                return this.SuccessFunction();

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByShowType([FromBody] GetAllByShowTypeParam param)
        {
            try
            {
                var pageSize = 5;
                var query = param.q;

                var notis = db.Notifications
                    .Where(c => (c.ShowType == (NotificationShowType)param.showType || c.ShowType == NotificationShowType.Both) && c.IsShow == true);

                if (!string.IsNullOrEmpty(query))
                {
                    notis = notis.Where(c => c.Title.Contains(query) || c.Content.Contains(query) || c.ShortContent.Contains(query));
                }

                var count = await notis.CountAsync();

                var nts = await notis.OrderByDescending(c => c.CreateDate)
                    .Skip((param.page - 1) * pageSize)
                    .Take(pageSize)
                .ToListAsync();

                return this.DataFunction(true, new
                {
                    notis = nts,
                    count = count
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllLastWeek([FromBody] NotificationShowType showType)
        {
            try
            {
                var notis = await db.Notifications
                    .Where(c => (c.ShowType == showType || c.ShowType == NotificationShowType.Both) && c.IsShow == true)
                    .Where(c => c.CreateDate >= DateTime.Now.AddDays(-7))
                .ToListAsync();

                return this.DataFunction(true, notis);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setNotificationSeen([FromBody] NotificationSeenParam param)
        {
            try
            {
                foreach (var id in param.NotificationIds)
                {
                    if (await db.NotificationSeens.AnyAsync(c => c.NotificationId == id && c.AgentId == param.AgentId && c.AgentType == param.AgentType))
                    {
                        continue;
                    }

                    var notiSeen = new NotificationSeen
                    {
                        AgentId = param.AgentId,
                        AgentType = param.AgentType,
                        AgentName = param.AgentName,
                        NotificationId = id,
                        Date = DateTime.Now,
                        ClassId = param.ClassId,
                        GradeId = param.GradeId
                    };

                    await db.NotificationSeens.AddAsync(notiSeen);
                }

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetNotificationSeens([FromBody] GetNotificationSeensParam param)
        {
            try
            {
                var notiSeen = db.NotificationSeens.Where(c => c.NotificationId == param.notificationId)
                    .Select(c => new
                    {
                        agentFullName = c.getAgentFullName(c.AgentType, c.AgentName),
                        dateString = c.Date.ToPersianDateWithTime(),
                        Date = c.Date,
                        GradeId = c.GradeId,
                        ClassId = c.ClassId
                    })
                .AsQueryable();

                if (param.gradeId.HasValue)
                {
                    notiSeen = notiSeen.Where(c => c.GradeId == param.gradeId.Value);
                }

                if (param.classId.HasValue)
                {
                    notiSeen = notiSeen.Where(c => c.ClassId == param.classId.Value);
                }

                return this.DataFunction(true, await notiSeen.OrderByDescending(c => c.Date).ToListAsync());
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }

    public class GetNotificationSeensParam
    {
        public int notificationId { get; set; }

        public int? gradeId { get; set; }

        public int? classId { get; set; }
    }

    public class NotificationSeenParam
    {
        public int[] NotificationIds { get; set; }

        public string AgentName { get; set; }

        public int AgentId { get; set; }

        public int AgentType { get; set; }
        public int ClassId { get; set; }
        public int GradeId { get; set; }
    }

    public class getNotificationAgentsParam
    {
        public int showType { get; set; }

        public int? selectedGrade { get; set; }

        public int? selectedClass { get; set; }
    }

    public class GetAllByShowTypeParam
    {
        public int page { get; set; }

        public int showType { get; set; }

        public string q { get; set; }
    }

    public class BrodcastParam
    {
        public int notificationId { get; set; }

        public int[] agentIds { get; set; }
    }

    public class SubscribeParam
    {
        public PushSubscription sub { get; set; }

        public int stdId { get; set; }

        public bool isParent { get; set; }
    }
}