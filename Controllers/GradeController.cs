using System.Collections.Generic;
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
    public class GradeController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public GradeController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Grade grade)
        {
            try
            {
                await db.Grades.AddAsync(grade);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Grade grade)
        {
            try
            {
                var grd = await db.Grades.SingleAsync(c => c.Id == grade.Id);

                grd.Capasity = grade.Capasity;
                grd.Code = grade.Code;
                grd.Desc = grade.Desc;
                grd.InternalCode = grade.InternalCode;
                grd.Name = grade.Name;
                grd.Order = grade.Order;
                grd.OrgCode = grade.OrgCode;
                grd.TituteId = grade.TituteId;
                grd.YeareducationId = grade.YeareducationId;

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

                var grd = db.Grades
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    grd = grd.Where(c => c.Name.Contains(query) || c.Desc.Contains(query) ||
                                    c.Code.Contains(query) || c.Capasity.ToString().Contains(query) ||
                                    c.InternalCode.Contains(query) || c.Order.ToString().Contains(query) ||
                                    c.OrgCode.Contains(query) || c.Yeareducation.Name.Contains(query) ||
                                    c.Titute.Name.Contains(query));

                }

                count = grd.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        grd = grd.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        grd = grd.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        grd = grd.OrderBy(c => c.Code);
                    }
                    if (getparams.sort.Equals("tituteName"))
                    {
                        grd = grd.OrderBy(c => c.tituteName);
                    }
                    if (getparams.sort.Equals("yeareducationName"))
                    {
                        grd = grd.OrderBy(c => c.yeareducationName);
                    }
                    if (getparams.sort.Equals("capasity"))
                    {
                        grd = grd.OrderBy(c => c.Capasity);
                    }
                    if (getparams.sort.Equals("desc"))
                    {
                        grd = grd.OrderBy(c => c.Desc);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        grd = grd.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        grd = grd.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        grd = grd.OrderByDescending(c => c.Code);
                    }
                    if (getparams.sort.Equals("tituteName"))
                    {
                        grd = grd.OrderByDescending(c => c.tituteName);
                    }
                    if (getparams.sort.Equals("yeareducationName"))
                    {
                        grd = grd.OrderByDescending(c => c.yeareducationName);
                    }
                    if (getparams.sort.Equals("capasity"))
                    {
                        grd = grd.OrderByDescending(c => c.Capasity);
                    }
                    if (getparams.sort.Equals("desc"))
                    {
                        grd = grd.OrderByDescending(c => c.Desc);
                    }
                }
                else
                {
                    grd = grd.OrderBy(c => c.Id);
                }

                grd = grd.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                grd = grd.Take(getparams.pageSize);

                var q = await grd
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code,
                        tituteName = c.Titute.Name,
                        yeareducationName = c.Yeareducation.Name,
                        Capasity = c.Capasity,
                        haveClass = c.Classes.Any(),
                        haveCourse = c.Courses.Any()
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

                var nowYeareducationId = await this.getActiveYeareducationId();

                var grd = await db.Grades
                    .Select(c => new
                    {
                        id = c.Id,
                        name = c.Name,
                        YeareducationId = c.YeareducationId,
                        TituteId = c.TituteId
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();

                return this.DataFunction(true, grd);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByYeareducation([FromBody] int yeareducationId)
        {
            try
            {
                var grd = await db.Grades.Where(c => c.YeareducationId == yeareducationId).Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, grd);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByTeacher([FromBody] int teacherId)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var courses = await db.Courses
                    .Where(c => c.TeacherId == teacherId)
                        .Include(c => c.Grade)
                .ToListAsync();

                var grades = new List<Grade>();

                foreach (var course in courses)
                {
                    var grade = course.Grade;

                    if (!grades.Any(c => c.Id == grade.Id))
                    {
                        grades.Add(grade);
                    }
                }

                return this.DataFunction(true, grades.Where(c => c.YeareducationId == nowYeareducationId));
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getGrade([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var grd = await db.Grades.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, grd);
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

                        var grd = await db.Grades.Include(c => c.Classes)
                            .SingleAsync(c => c.Id == id);

                        if (grd == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (grd.haveClass)
                        {
                            return this.UnSuccessFunction("این مقطع تحصیلی دارای کلاس هایی است", "error");
                        }

                        db.Grades.Remove(grd);
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
        public async Task<IActionResult> getPrintData()
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var headers = new List<string>();
                var datas = new List<List<string>>();

                var title = "لیست مقاطع تحصیلی";


                headers.Add("ردیف");
                headers.Add("کد مقطع تحصیلی");
                headers.Add("عنوان");
                headers.Add("آموزشگاه");
                headers.Add("سال تحصیلی");
                headers.Add("ظرفیت پذیرش");

                var grades = await db.Grades
                    .Include(c => c.Titute)
                    .Include(c => c.Yeareducation)
                .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();

                grades.ForEach(c =>
                {
                    var lstData = new List<string>();

                    lstData.Add(c.Id.ToString());
                    lstData.Add(c.Code.ToString());
                    lstData.Add(c.Name);
                    lstData.Add(c.Titute.Name);
                    lstData.Add(c.Yeareducation.Name);
                    lstData.Add(c.Capasity.ToString());

                    datas.Add(lstData);
                });

                return this.DataFunction(true, new PrintData
                {
                    headers = headers,
                    datas = datas,
                    title = title
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }
}