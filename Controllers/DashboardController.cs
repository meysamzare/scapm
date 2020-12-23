using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class DashboardController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public DashboardController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> getUpcommingExams()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                var exams = db.Exams.Where(c => c.Date >= DateTime.Now && c.IsCancelled == false)
                .OrderByDescending(c => c.Date)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    gradeName = c.Grade.Name,
                    className = c.Class.Name,
                    teacherName = c.Teacher.Name,
                    dateString = c.Date.ToPersianDate(),
                    Result = c.Result,
                    yeareducationId = c.Grade.YeareducationId
                })
                .Where(c => c.yeareducationId == nowYeareducationId);

                var totalCount = await exams.CountAsync();

                var examTen = await exams.Take(10).ToListAsync();

                return this.DataFunction(true, new
                {
                    totalCount = totalCount,
                    exams = examTen
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getPassedExams()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                var exams = db.Exams.Where(c => c.Date <= DateTime.Now && c.IsCancelled == false)
                .OrderByDescending(c => c.Date)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    gradeName = c.Grade.Name,
                    className = c.Class.Name,
                    teacherName = c.Teacher.Name,
                    dateString = c.Date.ToPersianDate(),
                    Result = c.Result,
                    yeareducationId = c.Grade.YeareducationId
                })
                .Where(c => c.yeareducationId == nowYeareducationId);

                var totalCount = await exams.CountAsync();

                var examTen = await exams.Take(10).ToListAsync();

                return this.DataFunction(true, new
                {
                    totalCount = totalCount,
                    exams = examTen
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getWaitingForResultExams()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                var exams = db.Exams.Where(c => c.Result == false && c.IsCancelled == false)
                .OrderByDescending(c => c.Date)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    gradeName = c.Grade.Name,
                    className = c.Class.Name,
                    teacherName = c.Teacher.Name,
                    dateString = c.Date.ToPersianDate(),
                    Result = c.Result,
                    yeareducationId = c.Grade.YeareducationId
                })
                .Where(c => c.yeareducationId == nowYeareducationId);

                var totalCount = await exams.CountAsync();

                var examTen = await exams.Take(10).ToListAsync();


                return this.DataFunction(true, new
                {
                    totalCount = totalCount,
                    exams = examTen
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getCancelledExams()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                var exams = db.Exams.Where(c => c.IsCancelled == true)
                .OrderByDescending(c => c.Date)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    gradeName = c.Grade.Name,
                    className = c.Class.Name,
                    teacherName = c.Teacher.Name,
                    dateString = c.Date.ToPersianDate(),
                    Result = c.Result,
                    yeareducationId = c.Grade.YeareducationId
                })
                .Where(c => c.yeareducationId == nowYeareducationId);

                var totalCount = await exams.CountAsync();

                var examTen = await exams.Take(10).ToListAsync();


                return this.DataFunction(true, new
                {
                    totalCount = totalCount,
                    exams = examTen
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getFirstTituteName()
        {
            try
            {
                if (await db.InsTitutes.AnyAsync())
                {
                    var firstInsTitute = await db.InsTitutes.OrderBy(c => c.Id).FirstOrDefaultAsync();
                    return this.DataFunction(true, firstInsTitute.Name);
                }

                return this.DataFunction(true, "----");

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getActiveYeareducationName()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                var activeYeareducation = await db.Yeareducations.FirstOrDefaultAsync(c => c.Id == nowYeareducationId);

                if (activeYeareducation == null)
                {
                    return this.DataFunction(true, "----");
                }

                return this.DataFunction(true, activeYeareducation.Name);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public IActionResult getActiveYeareducationId()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                return this.DataFunction(true, nowYeareducationId);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getRegistredStudentCountInActiveYeareducation()
        {
            try
            {
                var nowYeareducationId = this.getActiveYeareducationIdNonAsync();

                var stdClassMngs = db.StdClassMngs.Where(c => c.YeareducationId == nowYeareducationId);

                var count = await stdClassMngs.CountAsync();

                return this.DataFunction(true, count);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getTicketCount([FromBody] getTickets param)
        {
            try
            {
                var tickets = db.Tickets
                .Where(c => (c.SenderId == param.id && c.SenderType == param.type)
                        || (c.ReciverId == param.id && c.ReciverType == param.type));

                var count = await tickets.CountAsync();

                return this.DataFunction(true, count);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getChatCount()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(c => c.Username == User.Identity.Name);

                var chatsCount = await db.Chats.Where(c => c.SenderId == user.Id).CountAsync();

                return this.DataFunction(true, chatsCount);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getUpCommingSchedules()
        {
            try
            {
                var nowDate = DateTime.Now;

                var schedules = db.Schedules.Where(c => c.isOver == false);

                var totalCount = await schedules.CountAsync();

                var lst = await schedules.OrderBy(c => c.DateStart).Take(5).ToListAsync();

                return this.DataFunction(true, new
                {
                    schedules = lst,
                    totalCount = totalCount
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }

}