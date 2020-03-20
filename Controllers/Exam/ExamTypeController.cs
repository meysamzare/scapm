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
    public class ExamTypeController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public ExamTypeController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ExamType examType)
        {
            try
            {
                await db.ExamTypes.AddAsync(examType);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ExamType examType)
        {
            try
            {
                var extp = await db.ExamTypes.SingleAsync(c => c.Id == examType.Id);

                extp.Name = examType.Name;
                extp.Desc = examType.Desc;

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

                var extp = db.ExamTypes.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    extp = extp.Where(c => c.Name.Contains(query) || c.Desc.Contains(query));

                }

                count = extp.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        extp = extp.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        extp = extp.OrderBy(c => c.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        extp = extp.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        extp = extp.OrderByDescending(c => c.Name);
                    }
                }
                else
                {
                    extp = extp.OrderBy(c => c.Id);
                }

                extp = extp.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                extp = extp.Take(getparams.pageSize);

                var q = await extp
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        haveExam = c.Exams.Any()
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

                var extp = await db.ExamTypes.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, extp);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getExamType([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var extp = await db.ExamTypes.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, extp);
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

                        var extp = await db.ExamTypes.Include(c => c.Exams)
                            .SingleAsync(c => c.Id == id);

                        if (extp == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (extp.haveExam)
                        {
                            return this.UnSuccessFunction(" نوع آزمونٍ " + extp.Name + " دارای آزمون هایی است", "error");
                        }

                        db.ExamTypes.Remove(extp);
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