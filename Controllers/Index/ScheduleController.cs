using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;
using SCMR_Api.Model.Financial;
using SCMR_Api.Model.Index;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ScheduleController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public ScheduleController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Schedule schedule)
        {
            try
            {
                schedule.DateStart = schedule.DateStart.AddDays(1);
                schedule.DateEnd = schedule.DateEnd.AddDays(1);


                if (!string.IsNullOrEmpty(schedule.PicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, schedule.PicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(schedule.PicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    schedule.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + schedule.PicName);
                }


                await db.Schedules.AddAsync(schedule);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Schedule schedule)
        {
            try
            {
                var sch = await db.Schedules.FirstOrDefaultAsync(c => c.Id == schedule.Id);

                var datePB = sch.DateStart;

                sch.DateStart = schedule.DateStart;

                if (datePB != schedule.DateStart)
                {
                    sch.DateStart = sch.DateStart.AddDays(1);
                }

                var dateEB = sch.DateEnd;

                sch.DateEnd = schedule.DateEnd;

                if (dateEB != schedule.DateEnd)
                {
                    sch.DateEnd = sch.DateEnd.AddDays(1);
                }


                if (!string.IsNullOrEmpty(schedule.PicData))
                {
                    if (!string.IsNullOrEmpty(sch.PicUrl))
                    {
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + sch.PicUrl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, schedule.PicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(schedule.PicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    sch.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + schedule.PicName);
                }

                sch.PostId = schedule.PostId;
                sch.Title = schedule.Title;
                sch.Content = schedule.Content;

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

                var sl = db.Schedules
                    .Include(c => c.Post)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    sl = sl.Where(c => c.Title.Contains(query) || c.Content.Contains(query) || c.Post.Name.Contains(query));

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
                    if (getparams.sort.Equals("post"))
                    {
                        sl = sl.OrderBy(c => c.Post.Name);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.DateStart).ThenBy(c => c.DateEnd);
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
                    if (getparams.sort.Equals("post"))
                    {
                        sl = sl.OrderByDescending(c => c.Post.Name);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.DateStart).ThenByDescending(c => c.DateEnd);
                    }
                }
                else
                {
                    sl = sl.OrderBy(c => c.Id);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        postName = c.Post.Name,
                        dateStartString = c.DateStart.ToPersianDate(),
                        dateEndString = c.DateEnd.ToPersianDate()
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

                var sl = await db.Schedules.Select(c => new { id = c.Id, title = c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getSchedulesIndex()
        {
            try
            {
                //&& DateTime.Now < c.DateEnd.AddDays(1)
                var schedules = await db.Schedules
                        .Where(c => (DateTime.Now > c.DateStart)).OrderByDescending(c => c.DateStart).Take(10)
                    .Include(c => c.Post)
                .ToListAsync();

                return this.DataFunction(true, schedules);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getSchedule([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Schedules.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.Schedules
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.Schedules.Remove(sl);
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