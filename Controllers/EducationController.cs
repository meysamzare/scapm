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
    public class EducationController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public EducationController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Education education)
        {
            try
            {
                await db.Educations.AddAsync(education);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Education education)
        {
            try
            {
                var edu = await db.Educations.SingleAsync(c => c.Id == education.Id);

                edu.Name = education.Name;

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

                var edu = db.Educations.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    edu = edu.Where(c => c.Name.Contains(query));

                }

                count = edu.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        edu = edu.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        edu = edu.OrderBy(c => c.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        edu = edu.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        edu = edu.OrderByDescending(c => c.Name);
                    }
                }
                else
                {
                    edu = edu.OrderBy(c => c.Id);
                }

                edu = edu.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                edu = edu.Take(getparams.pageSize);

                var q = await edu
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        havePerson = c.OrgPeople.Any()
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

                var edu = await db.Educations.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, edu);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getEducation([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var edu = await db.Educations.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, edu);
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

                        var edu = await db.Educations.Include(c => c.OrgPeople)
                            .SingleAsync(c => c.Id == id);

                        if (edu == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (edu.havePerson)
                        {
                            return this.UnSuccessFunction(" مدرک تحصیلی " + edu.Name + " دارای افرادی است", "error");
                        }

                        db.Educations.Remove(edu);
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