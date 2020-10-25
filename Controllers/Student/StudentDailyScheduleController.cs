using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StudentDailyScheduleController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IConfiguration _config;

        public StudentDailyScheduleController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentDailyScheduleAddParam param)
        {
            try
            {
                if (db.StdClassMngs.First(c => c.Id == param.studentDailySchedule.StdClassMngId).StudentId != param.stdId)
                {
                    return this.UnSuccessFunction("لطفا مقادیر ورودی را کنترل نمایید");
                }

                param.studentDailySchedule.DateCreate = DateTime.Now;
                param.studentDailySchedule.DateExecute = param.studentDailySchedule.DateExecute.AddDays(1);
                param.studentDailySchedule.State = StudentDailyScheduleState.NotLooked;

                db.StudentDailySchedules.Add(param.studentDailySchedule);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] StudentDailySchedule param)
        {
            try
            {
                db.Update(param);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetState([FromBody] SetStateParam param)
        {
            try
            {
                var SDS = await db.StudentDailySchedules.FirstOrDefaultAsync(c => c.Id == param.id);

                SDS.State = (StudentDailyScheduleState)param.state;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByStd([FromBody] getAllStudentDailyScheduleByStdParam param)
        {
            try
            {
                var stdClassMng = await db.StdClassMngs.FirstAsync(c => c.Id == param.stdClassMngId);
                if (stdClassMng.StudentId != param.studentId)
                {
                    return this.UnSuccessFunction("Invalid Request");
                }

                var SDSs = await db.StudentDailySchedules
                    .Where(c => c.StdClassMngId == param.stdClassMngId)
                .Include(c => c.Course)
                .OrderByDescending(c => c.Id)
                    .ThenBy(c => c.State)
                .ToListAsync();

                return this.DataFunction(true, SDSs);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getStudentDailySchedule([FromBody] int id)
        {
            try
            {
                var SDS = await db.StudentDailySchedules
                    .Include(c => c.Course)
                .FirstAsync(c => c.Id == id);

                return this.DataFunction(true, SDS);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> getStudentDailyScheduleWithCourses([FromBody] int id)
        {
            try
            {
                var SDS = await db.StudentDailySchedules
                    .Include(c => c.Course)
                .FirstAsync(c => c.Id == id);

                var courses = await db.Courses.Where(c => c.GradeId == SDS.Course.GradeId).ToListAsync();

                return this.DataFunction(true, new
                {
                    sds = SDS,
                    courses = courses
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setParentComment([FromBody] setCommentParam param)
        {
            try
            {
                var SDS = await db.StudentDailySchedules.FirstOrDefaultAsync(c => c.Id == param.id);

                SDS.StudentParentComment = param.comment;
                SDS.StudentParentCommentDate = DateTime.Now;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setConsultantComment([FromBody] setCommentParam param)
        {
            try
            {
                var SDS = await db.StudentDailySchedules.FirstOrDefaultAsync(c => c.Id == param.id);

                SDS.ConsultantComment = param.comment;
                SDS.ConsultantCommentDate = DateTime.Now;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


    }

    public class setCommentParam
    {
        public int id { get; set; }
        public string comment { get; set; }
    }

    public class getAllStudentDailyScheduleByStdParam
    {
        public int studentId { get; set; }
        public int stdClassMngId { get; set; }
    }

    public class SetStateParam
    {
        public int id { get; set; }
        public int state { get; set; }
    }

    public class StudentDailyScheduleAddParam
    {
        public StudentDailySchedule studentDailySchedule { get; set; }

        public int stdId { get; set; }
    }
}