using System;
using System.Collections.Generic;
using System.IO;
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
    public class OnlineClassController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IHttpContextAccessor accessor;

        public OnlineClassController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IHttpContextAccessor _accessor)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            accessor = _accessor;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OnlineClass onlineClass)
        {
            try
            {
                await db.OnlineClasses.AddAsync(onlineClass);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] OnlineClass onlineClass)
        {
            try
            {
                var dbOnlineClass = await db.OnlineClasses.FirstOrDefaultAsync(c => c.Id == onlineClass.Id);

                dbOnlineClass.meetingId = onlineClass.meetingId;
                dbOnlineClass.name = onlineClass.name;
                dbOnlineClass.GradeId = onlineClass.GradeId;
                dbOnlineClass.ClassId = onlineClass.ClassId;
                dbOnlineClass.CourseId = onlineClass.CourseId;
                dbOnlineClass.OnlineClassServerId = onlineClass.OnlineClassServerId;
                dbOnlineClass.AuthorizeType = onlineClass.AuthorizeType;
                dbOnlineClass.AllowedAdminIds = onlineClass.AllowedAdminIds;
                dbOnlineClass.AllowedStudentIds = onlineClass.AllowedStudentIds;
                dbOnlineClass.attendeePW = onlineClass.attendeePW;
                dbOnlineClass.moderatorPW = onlineClass.moderatorPW;
                dbOnlineClass.welcome = onlineClass.welcome;
                dbOnlineClass.maxParticipants = onlineClass.maxParticipants;
                dbOnlineClass.duration = onlineClass.duration;
                dbOnlineClass.logoutURL = onlineClass.logoutURL;
                dbOnlineClass.meta = onlineClass.meta;
                dbOnlineClass.copyright = onlineClass.copyright;
                dbOnlineClass.parentMeetingID = onlineClass.parentMeetingID;
                dbOnlineClass.sequence = onlineClass.sequence;
                dbOnlineClass.record = onlineClass.record;
                dbOnlineClass.isBreakout = onlineClass.isBreakout;
                dbOnlineClass.freeJoin = onlineClass.freeJoin;
                dbOnlineClass.autoStartRecording = onlineClass.autoStartRecording;
                dbOnlineClass.allowStartStopRecording = onlineClass.allowStartStopRecording;
                dbOnlineClass.webcamsOnlyForModerator = onlineClass.webcamsOnlyForModerator;
                dbOnlineClass.muteOnStart = onlineClass.muteOnStart;
                dbOnlineClass.allowModsToUnmuteUsers = onlineClass.allowModsToUnmuteUsers;
                dbOnlineClass.lockSettingsDisableCam = onlineClass.lockSettingsDisableCam;
                dbOnlineClass.lockSettingsDisableMic = onlineClass.lockSettingsDisableMic;
                dbOnlineClass.lockSettingsDisablePrivateChat = onlineClass.lockSettingsDisablePrivateChat;
                dbOnlineClass.lockSettingsDisablePublicChat = onlineClass.lockSettingsDisablePublicChat;
                dbOnlineClass.lockSettingsDisableNote = onlineClass.lockSettingsDisableNote;
                dbOnlineClass.lockSettingsLockedLayout = onlineClass.lockSettingsLockedLayout;
                dbOnlineClass.lockSettingsLockOnJoin = onlineClass.lockSettingsLockOnJoin;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] onlineClassGetParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.OnlineClasses
                    .Include(c => c.Grade)
                    .Include(c => c.Class)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.name.Contains(query) || c.className.Contains(query) || c.gradeName.Contains(query));
                }

                if (param.selectedGrade.HasValue)
                {
                    sl = sl.Where(c => c.AuthorizeType == OnlineClassAuthorizeType.ByClass && c.GradeId == param.selectedGrade.Value);
                }

                if (param.selectedClass.HasValue)
                {
                    sl = sl.Where(c => c.AuthorizeType == OnlineClassAuthorizeType.ByClass && c.ClassId == param.selectedClass.Value);
                }

                if (param.selectedCourse.HasValue)
                {
                    sl = sl.Where(c => c.AuthorizeType == OnlineClassAuthorizeType.ByClass && c.CourseId == param.selectedCourse.Value);
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        sl = sl.OrderBy(c => c.name);
                    }
                    if (getparams.sort.Equals("grade"))
                    {
                        sl = sl.OrderBy(c => c.Grade.Name).ThenBy(c => c.Class.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        sl = sl.OrderByDescending(c => c.name);
                    }
                    if (getparams.sort.Equals("grade"))
                    {
                        sl = sl.OrderByDescending(c => c.Grade.Name).ThenByDescending(c => c.Class.Name);
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
                        id = c.Id,
                        name = c.name,
                        gradeName = c.Grade != null && c.AuthorizeType == OnlineClassAuthorizeType.ByClass ? c.Grade.Name : "---",
                        className = c.Class != null && c.AuthorizeType == OnlineClassAuthorizeType.ByClass ? c.Class.Name : "---",
                        meetingId = c.meetingId,
                        authorizeType = (int)c.AuthorizeType,
                        serverId = c.OnlineClassServerId
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
        public async Task<IActionResult> getAll()
        {
            try
            {

                var sl = await db.OnlineClasses.Select(c => new { id = c.Id, name = c.name }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByGrade_Class_Course([FromBody] getAllByGrade_Class_Course param)
        {
            try
            {
                var onlineClassess = new List<OnlineClass>();

                var onlineClassesByClass = db.OnlineClasses.Where(c => c.AuthorizeType == OnlineClassAuthorizeType.ByClass).AsQueryable();

                var onlineClassesByCustom = db.OnlineClasses.Where(c => c.AuthorizeType == OnlineClassAuthorizeType.Custom).AsQueryable();


                if (param.gradeId.HasValue)
                {
                    onlineClassesByClass = onlineClassesByClass.Where(c => c.GradeId == param.gradeId.Value);
                }
                if (param.classId.HasValue)
                {
                    onlineClassesByClass = onlineClassesByClass.Where(c => c.ClassId.HasValue ? c.ClassId.Value == param.classId.Value : false);
                }
                if (param.courseId.HasValue)
                {
                    onlineClassesByClass = onlineClassesByClass.Where(c => c.CourseId == param.courseId.Value);
                }


                if (param.isAdmin)
                {
                    if (!param.gradeId.HasValue && !param.classId.HasValue && !param.courseId.HasValue)
                    {
                        onlineClassesByCustom = onlineClassesByCustom.Where(c => c.AllowedAdminIds.Contains(param.userId));
                        onlineClassess.AddRange(await onlineClassesByCustom.ToListAsync());
                    }
                    else
                    {
                        onlineClassess.AddRange(await onlineClassesByClass.ToListAsync());
                    }
                }
                else
                {
                    onlineClassesByCustom = onlineClassesByCustom.Where(c => c.AllowedStudentIds.Contains(param.userId));
                    onlineClassess.AddRange(await onlineClassesByCustom.ToListAsync());
                    onlineClassess.AddRange(await onlineClassesByClass.ToListAsync());
                }

                return this.DataFunction(true, onlineClassess);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getOnlineClassByMeetingId([FromBody] string meetingId)
        {
            try
            {
                var onlineClass = await db.OnlineClasses.Where(c => c.meetingId == meetingId).FirstOrDefaultAsync();

                return this.DataFunction(true, onlineClass);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getOnlineClassServerId([FromBody] string meetingId)
        {
            try
            {
                var onlineClass = await db.OnlineClasses.FirstOrDefaultAsync(c => c.meetingId == meetingId);

                var serverId = 0;

                if (onlineClass != null)
                {
                    serverId = onlineClass.OnlineClassServerId.Value;
                }

                return this.DataFunction(true, serverId);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllowedStudentIds([FromBody] string meetingId)
        {
            try
            {
                var onlineClass = await db.OnlineClasses.Where(c => c.meetingId == meetingId)
                    .Include(c => c.Grade)
                    .Include(c => c.Class)
                .FirstOrDefaultAsync();

                var allowedStudentIds = new List<int>();

                if (onlineClass.AuthorizeType == OnlineClassAuthorizeType.Custom)
                {
                    allowedStudentIds.AddRange(onlineClass.AllowedStudentIds);
                }
                else
                {
                    var stdClassMngs = db.StdClassMngs.Where(c => c.GradeId == onlineClass.GradeId && c.YeareducationId == onlineClass.Grade.YeareducationId);

                    if (onlineClass.ClassId.HasValue)
                    {
                        stdClassMngs = stdClassMngs.Where(c => c.ClassId == onlineClass.ClassId);
                    }

                    var studentIds = await stdClassMngs.Select(c => c.Student.Id).Distinct().ToListAsync();

                    allowedStudentIds.AddRange(studentIds);
                }

                return this.DataFunction(true, allowedStudentIds);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        [HttpPost]
        public async Task<IActionResult> getOnlineClass([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.OnlineClasses.FirstOrDefaultAsync(c => c.Id == id);

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
        public async Task<IActionResult> setLogin([FromBody] setLoginParam param)
        {
            try
            {
                var onlineClass = await db.OnlineClasses.FirstOrDefaultAsync(c => c.meetingId == param.meetingId);

                var onlineClassLogin = new OnlineClassLogin
                {
                    Id = 0,
                    OnlineClassId = onlineClass.Id,
                    Date = DateTime.Now,
                    FullName = param.fullName,
                    UserName = param.userName,
                    IP = accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserId = param.userId,
                    AgentType = (OnlineClassAgentType)param.agentType,
                };

                await db.OnlineClassLogins.AddAsync(onlineClassLogin);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getLogins([FromBody] string meetingId)
        {
            try
            {
                var onlineClass = await db.OnlineClasses.Include(c => c.Grade).FirstOrDefaultAsync(c => c.meetingId == meetingId);
                var onlineClassId = onlineClass.Id;

                var onlineClassLogins = await db.OnlineClassLogins
                    .Where(c => c.OnlineClassId == onlineClassId && c.Date.Date == DateTime.Now.Date)
                .Select(c => new
                {
                    id = c.Id,
                    fullName = c.FullName,
                    username = c.UserName,
                    userId = c.UserId,
                    onlineClassAgentTypeString = Enum.GetName(typeof(OnlineClassAgentType), c.AgentType),
                    onlineClassAgentType = (int)c.AgentType,
                    dateString = c.Date.ToPersianDateWithTime(),
                    ip = c.IP,
                    absence = false,
                }).ToListAsync();


                var allowedStudentIds = new List<int>();

                if (onlineClass.AuthorizeType == OnlineClassAuthorizeType.Custom)
                {
                    allowedStudentIds.AddRange(onlineClass.AllowedStudentIds);
                }
                else
                {
                    var stdClassMngs = db.StdClassMngs.Where(c => c.GradeId == onlineClass.GradeId && c.YeareducationId == onlineClass.Grade.YeareducationId);

                    if (onlineClass.ClassId.HasValue)
                    {
                        stdClassMngs = stdClassMngs.Where(c => c.ClassId == onlineClass.ClassId);
                    }

                    var studentIds = await stdClassMngs.Select(c => c.Student.Id).Distinct().ToListAsync();

                    allowedStudentIds.AddRange(studentIds);
                }

                var peresentStudentIds = onlineClassLogins.Where(c => c.onlineClassAgentType == 2).Select(c => c.userId);

                foreach (var absenceStdId in allowedStudentIds.Where(c => !peresentStudentIds.Contains(c)))
                {
                    var student = await db.Students.FirstOrDefaultAsync(c => c.Id == absenceStdId);

                    onlineClassLogins.Add(new
                    {
                        id = (long)0,
                        fullName = student.LastName + " - " + student.Name,
                        username = student.IdNumber2,
                        userId = student.Id,
                        onlineClassAgentTypeString = "PMA",
                        onlineClassAgentType = 2,
                        dateString = "غائب",
                        ip = "---",
                        absence = true,
                    });
                }

                return this.DataFunction(true, onlineClassLogins.OrderByDescending(c => c.id).ThenBy(c => c.fullName));
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

                        var sl = await db.OnlineClasses
                        .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.OnlineClasses.Remove(sl);
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


    }

    public class onlineClassGetParam
    {
        public getparams getparams { get; set; }

        public int? selectedGrade { get; set; }
        public int? selectedClass { get; set; }
        public int? selectedCourse { get; set; }
    }

    public class getLoginsParam
    {
    }

    public class setLoginParam
    {
        public string meetingId { get; set; }
        public string fullName { get; set; }
        public string userName { get; set; }
        public int userId { get; set; }
        public int agentType { get; set; }
    }

    public class getAllByGrade_Class_Course
    {
        public int? gradeId { get; set; }
        public int? classId { get; set; }
        public int? courseId { get; set; }
        public bool isAdmin { get; set; }
        public int userId { get; set; }
    }
}