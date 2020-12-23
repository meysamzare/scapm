using System;
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
    public class YeareducationController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public YeareducationController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Yeareducation yeareducation)
        {
            try
            {
                yeareducation.DateEnd = yeareducation.DateEnd.AddDays(1);
                yeareducation.DateStart = yeareducation.DateStart.AddDays(1);

                await db.Yeareducations.AddAsync(yeareducation);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getYeareducation([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var tit = await db.Yeareducations.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, tit);
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
        public async Task<IActionResult> getAll()
        {
            try
            {

                var tits = await db.Yeareducations
                    .Select(c => new { id = c.Id, name = c.Name, isActive = c.IsActive })
                .ToListAsync();

                return this.DataFunction(true, tits);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditYearParam yeareducation)
        {
            try
            {
                var year = await db.Yeareducations.SingleAsync(c => c.Id == yeareducation.id);

                var datestbefore = year.DateStart;
                var dateedbefore = year.DateEnd;

                year.Name = yeareducation.name;
                year.Desc = yeareducation.desc;
                year.ScoreType = (YeareducationScoreType)yeareducation.scoreType;


                year.DateStart = yeareducation.dateStart;
                year.DateEnd = yeareducation.dateEnd;

                if (datestbefore != yeareducation.dateStart)
                {
                    year.DateStart = year.DateStart.AddDays(1);
                }

                if (dateedbefore != yeareducation.dateEnd)
                {
                    year.DateEnd = year.DateEnd.AddDays(1);
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
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var year = await db.Yeareducations
                            .SingleAsync(c => c.Id == id);

                        if (year == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.Yeareducations.Remove(year);
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
        public async Task<IActionResult> Get([FromBody] getparams getparams)
        {
            try
            {
                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var year = db.Yeareducations.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    year = year.Where(c => c.Name.Contains(query) || c.Desc.Contains(query));

                }

                count = year.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        year = year.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        year = year.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("dateStart"))
                    {
                        year = year.OrderBy(c => c.DateStart);
                    }
                    if (getparams.sort.Equals("dateEnd"))
                    {
                        year = year.OrderBy(c => c.DateEnd);
                    }
                    if (getparams.sort.Equals("desc"))
                    {
                        year = year.OrderBy(c => c.Desc);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        year = year.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        year = year.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("dateStart"))
                    {
                        year = year.OrderByDescending(c => c.DateStart);
                    }
                    if (getparams.sort.Equals("dateEnd"))
                    {
                        year = year.OrderByDescending(c => c.DateEnd);
                    }
                    if (getparams.sort.Equals("desc"))
                    {
                        year = year.OrderByDescending(c => c.Desc);
                    }
                }
                else
                {
                    year = year.OrderBy(c => c.Id);
                }

                year = year.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                year = year.Take(getparams.pageSize);

                var q = await year
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        DateStartPersian = c.DateStart.ToPersianDate(),
                        DateEndPersian = c.DateEnd.ToPersianDate(),
                        haveExam = c.Exams.Any(),
                        haveGrade = c.Grades.Any(),
                        isActive = c.IsActive
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
        public async Task<IActionResult> setIsActive([FromBody] int id)
        {
            try
            {
                var yeareducations = db.Yeareducations.AsQueryable();

                await yeareducations.ForEachAsync(c => c.IsActive = false);

                var yeareducation = await db.Yeareducations.FirstOrDefaultAsync(c => c.Id == id);

                yeareducation.IsActive = true;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


    }

    public class EditYearParam
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime dateStart { get; set; }

        public DateTime dateEnd { get; set; }

        public string desc { get; set; }

        public int scoreType { get; set; }
    }
}