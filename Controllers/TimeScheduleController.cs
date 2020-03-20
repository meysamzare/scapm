using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TimeScheduleController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public TimeScheduleController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TimeSchedule timeSchedule)
        {
            try
            {
                await db.TimeSchedules.AddAsync(timeSchedule);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] TimeSchedule timeSchedule)
        {
            try
            {
                var tsch = await db.TimeSchedules.SingleAsync(c => c.Id == timeSchedule.Id);

                tsch.Name = timeSchedule.Name;
                tsch.CourseId = timeSchedule.CourseId;
                tsch.TeacherId = timeSchedule.TeacherId;
                tsch.TimeandDaysId = timeSchedule.TimeandDaysId;
                tsch.TimeStart = timeSchedule.TimeStart;
                tsch.TimeEnd = timeSchedule.TimeEnd;

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
                var nowYeareducationId = await this.getActiveYeareducationId();

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var tsch = db.TimeSchedules
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        TimeStart = c.TimeStart,
                        TimeEnd = c.TimeEnd,
                        courseTitle = c.Course.Name,
                        timeandDaysTitle = c.TimeandDays.Name,
                        teacherTitle = c.Teacher.Name,
                        YeareducationId = c.Course.Grade.YeareducationId
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    tsch = tsch.Where(c => c.Name.Contains(query) || c.TimeStart.ToString().Contains(query) ||
                                        c.TimeEnd.ToString().Contains(query) || c.courseTitle.Contains(query) ||
                                        c.teacherTitle.Contains(query));
                }

                count = tsch.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tsch = tsch.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tsch = tsch.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("time"))
                    {
                        tsch = tsch.OrderBy(c => c.TimeStart).ThenBy(c => c.TimeEnd);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tsch = tsch.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tsch = tsch.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("time"))
                    {
                        tsch = tsch.OrderByDescending(c => c.TimeStart).ThenByDescending(c => c.TimeEnd);
                    }
                }
                else
                {
                    tsch = tsch.OrderBy(c => c.Id);
                }

                tsch = tsch.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                tsch = tsch.Take(getparams.pageSize);

                var q = await tsch.ToListAsync();

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
                var nowYeareducationId = await this.getActiveYeareducationId();
                
                var tsch = await db.TimeSchedules
                    .Select(c => new { id = c.Id, name = c.Name, yeareducationId = c.Course.Grade.YeareducationId })
                    .Where(c => c.yeareducationId == nowYeareducationId)    
                .ToListAsync();

                return this.DataFunction(true, tsch);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getTimeSchedule([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var tsch = await db.TimeSchedules.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, tsch);
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

                        var tsch = await db.TimeSchedules
                            .SingleAsync(c => c.Id == id);

                        if (tsch == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.TimeSchedules.Remove(tsch);
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
}