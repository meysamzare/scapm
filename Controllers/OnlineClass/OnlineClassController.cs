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

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OnlineClassController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public OnlineClassController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OnlineClass onlineClass)
        {
            try
            {
                db.OnlineClasses.Add(onlineClass);

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
                db.OnlineClasses.Update(onlineClass);

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

                var sl = db.OnlineClasses
                    .Include(c => c.Grade)
                    .Include(c => c.Class)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.name.Contains(query) || c.className.Contains(query) || c.gradeName.Contains(query));
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
                        gradeName = c.Grade.Name,
                        className = c.Class.Name
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
                var onlineClasses = db.OnlineClasses.AsQueryable();

                
                if (param.gradeId.HasValue)
                {
                    onlineClasses = onlineClasses.Where(c => c.GradeId == param.gradeId.Value);
                }

                if (param.classId.HasValue)
                {
                    onlineClasses = onlineClasses.Where(c => c.ClassId.HasValue ? c.ClassId.Value == param.classId.Value : false);
                }

                if (param.courseId.HasValue)
                {
                    onlineClasses = onlineClasses.Where(c => c.CourseId == param.courseId.Value);
                }

                return this.DataFunction(true, await onlineClasses.ToListAsync());
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

    public class getAllByGrade_Class_Course
    {
        public int? gradeId { get; set; }
        public int? classId { get; set; }
        public int? courseId { get; set; }
    }
}