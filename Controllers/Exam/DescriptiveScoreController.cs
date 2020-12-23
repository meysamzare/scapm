using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DescriptiveScoreController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public DescriptiveScoreController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DescriptiveScore descriptiveScore)
        {
            try
            {
                var isaToOfList = await db.DescriptiveScores.AnyAsync(c => c.ToNumber == descriptiveScore.FromNumber);
                var isThereAnyFrom = await db.DescriptiveScores.AnyAsync(c => c.FromNumber == descriptiveScore.FromNumber);

                if (await db.DescriptiveScores.AnyAsync() && (!isaToOfList || isThereAnyFrom))
                {
                    return this.UnSuccessFunction("محدوده شروع باید با یکی از محدوده های پایان برابر باشد و همچنین نباید با " +
                        "هیچ کدام از محدوده های شروع برابر باشد!");
                }

                var isThereAnyTo = await db.DescriptiveScores.AnyAsync(c => c.ToNumber == descriptiveScore.ToNumber);

                if ((descriptiveScore.ToNumber <= descriptiveScore.FromNumber) || isThereAnyTo)
                {
                    return this.UnSuccessFunction("محدوده پایان باید از محدوده شروع بیشتر باشد و با هیچ یک از محدوده های پایان موجود برابر نباشد!");
                }

                await db.DescriptiveScores.AddAsync(descriptiveScore);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] DescriptiveScore descriptiveScore)
        {
            try
            {
                var DescriptiveScore = await db.DescriptiveScores.FirstOrDefaultAsync(c => c.Id == descriptiveScore.Id);

                var DescriptiveScoresList = db.DescriptiveScores.Except(db.DescriptiveScores.Where(c => c.Id == descriptiveScore.Id));

                var isaToOfList = await DescriptiveScoresList.AnyAsync(c => c.ToNumber == descriptiveScore.FromNumber);
                var isThereAnyFrom = await DescriptiveScoresList.AnyAsync(c => c.FromNumber == descriptiveScore.FromNumber);

                if (!isaToOfList || isThereAnyFrom)
                {
                    return this.UnSuccessFunction("محدوده شروع باید با یکی از محدوده های پایان برابر باشد و همچنین نباید با " +
                        "هیچ کدام از محدوده های شروع برابر باشد!");
                }

                var isThereAnyTo = await DescriptiveScoresList.AnyAsync(c => c.ToNumber == descriptiveScore.ToNumber);

                if ((descriptiveScore.ToNumber <= descriptiveScore.FromNumber) || isThereAnyTo)
                {
                    return this.UnSuccessFunction("محدوده پایان باید از محدوده شروع بیشتر باشد و با هیچ یک از محدوده های پایان موجود برابر نباشد!");
                }

                var descScore = await db.DescriptiveScores.FirstOrDefaultAsync(c => c.Id == descriptiveScore.Id);

                descScore.Name = descriptiveScore.Name;
                descScore.EnName = descriptiveScore.EnName;
                descScore.FromNumber = descriptiveScore.FromNumber;
                descScore.ToNumber = descriptiveScore.ToNumber;

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

                var descriptiveScores = db.DescriptiveScores.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    descriptiveScores = descriptiveScores.Where(c => c.Name.Contains(query) || c.EnName.Contains(query) || c.FromNumber.ToString().Contains(query) || c.ToNumber.ToString().Contains(query));
                }

                count = descriptiveScores.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        descriptiveScores = descriptiveScores.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        descriptiveScores = descriptiveScores.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("enName"))
                    {
                        descriptiveScores = descriptiveScores.OrderBy(c => c.EnName);
                    }
                    if (getparams.sort.Equals("fromNumber"))
                    {
                        descriptiveScores = descriptiveScores.OrderBy(c => c.FromNumber);
                    }
                    if (getparams.sort.Equals("toNumber"))
                    {
                        descriptiveScores = descriptiveScores.OrderBy(c => c.ToNumber);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        descriptiveScores = descriptiveScores.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        descriptiveScores = descriptiveScores.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("enName"))
                    {
                        descriptiveScores = descriptiveScores.OrderByDescending(c => c.EnName);
                    }
                    if (getparams.sort.Equals("fromNumber"))
                    {
                        descriptiveScores = descriptiveScores.OrderByDescending(c => c.FromNumber);
                    }
                    if (getparams.sort.Equals("toNumber"))
                    {
                        descriptiveScores = descriptiveScores.OrderByDescending(c => c.ToNumber);
                    }
                }
                else
                {
                    descriptiveScores = descriptiveScores.OrderBy(c => c.Id);
                }

                descriptiveScores = descriptiveScores.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                descriptiveScores = descriptiveScores.Take(getparams.pageSize);

                var q = await descriptiveScores
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        EnName = c.EnName,
                        FromNumber = c.FromNumber,
                        ToNumber = c.ToNumber
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
                var descriptiveScores = await db.DescriptiveScores.ToListAsync();

                return this.DataFunction(true, descriptiveScores);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getDescriptiveScore([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.DescriptiveScores.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.DescriptiveScores.FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.DescriptiveScores.Remove(sl);
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