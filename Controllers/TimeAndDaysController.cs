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
    public class TimeAndDaysController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public TimeAndDaysController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TimeandDays timeandDays)
        {
            try
            {
                await db.TimeandDays.AddAsync(timeandDays);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] TimeandDays timeandDays)
        {
            try
            {
                var tad = await db.TimeandDays.SingleAsync(c => c.Id == timeandDays.Id);

                tad.Name = timeandDays.Name;
                tad.sat = timeandDays.sat;
                tad.sun = timeandDays.sun;
                tad.mon = timeandDays.mon;
                tad.tue = timeandDays.tue;
                tad.wed = timeandDays.wed;
                tad.thr = timeandDays.thr;
                tad.fri = timeandDays.fri;

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

                var tad = db.TimeandDays.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    tad = tad.Where(c => c.Name.Contains(query));
                }

                count = tad.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tad = tad.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tad = tad.OrderBy(c => c.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tad = tad.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tad = tad.OrderByDescending(c => c.Name);
                    }
                }
                else
                {
                    tad = tad.OrderBy(c => c.Id);
                }

                tad = tad.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                tad = tad.Take(getparams.pageSize);

                var q = await tad
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        getNameOfSelectedDays = c.getNameOfSelectedDays,
                        haveTimeSchedules = c.TimeSchedules.Any()
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

                var tad = await db.TimeandDays.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, tad);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getTimeAndDays([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var tad = await db.TimeandDays.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, tad);
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

                        var tad = await db.TimeandDays.SingleAsync(c => c.Id == id);

                        if (tad == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (tad.haveTimeSchedules)
                        {
                            return this.UnSuccessFunction(" ایام هفته " + tad.Name + " دارای برنامه زمانی است", "error");
                        }

                        db.TimeandDays.Remove(tad);
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