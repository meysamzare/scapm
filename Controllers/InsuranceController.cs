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
    public class InsuranceController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public InsuranceController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Insurance insurance)
        {
            try
            {
                await db.Insurances.AddAsync(insurance);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Insurance insurance)
        {
            try
            {
                var ins = await db.Insurances.SingleAsync(c => c.Id == insurance.Id);

                ins.Name = insurance.Name;

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

                var ins = db.Insurances.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    ins = ins.Where(c => c.Name.Contains(query));

                }

                count = ins.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        ins = ins.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        ins = ins.OrderBy(c => c.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        ins = ins.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        ins = ins.OrderByDescending(c => c.Name);
                    }
                }
                else
                {
                    ins = ins.OrderBy(c => c.Id);
                }

                ins = ins.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                ins = ins.Take(getparams.pageSize);

                var q = await ins
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

                var ins = await db.Insurances.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, ins);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getInsurance([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var ins = await db.Insurances.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, ins);
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

                        var ins = await db.Insurances.Include(c => c.OrgPeople)
                            .SingleAsync(c => c.Id == id);

                        if (ins == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (ins.havePerson)
                        {
                            return this.UnSuccessFunction(" بیمه " + ins.Name + " دارای افرادی است", "error");
                        }

                        db.Insurances.Remove(ins);
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