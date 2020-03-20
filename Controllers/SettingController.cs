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
    public class SettingController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public SettingController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> setActiveYeareducationId([FromBody] int activeYeareducationId)
        {
            try
            {
                var yearId = await db.Settings.FirstOrDefaultAsync(c => c.Key == "NowYeareducationId");

                yearId.Value = activeYeareducationId.ToString();

                await db.SaveChangesAsync();

                var aa = await this.getActiveYeareducationId();

                return this.SuccessFunction(aa);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


    }
}