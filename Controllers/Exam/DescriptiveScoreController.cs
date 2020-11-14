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

                if (!isaToOfList || isThereAnyFrom)
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

                db.Update(DescriptiveScore);

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