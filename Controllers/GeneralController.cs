using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class GeneralController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public GeneralController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }



        [HttpPost]
        public IActionResult IsDateValid([FromBody] DateTime date)
        {
            try
            {
                if (date.Date == DateTime.Now.Date)
                {
                    return this.SuccessFunction(DateTime.Now.ToEnglishDate() + " یا "+ DateTime.Now.ToPersianDate());
                }

                return this.UnSuccessFunction(DateTime.Now.ToEnglishDate() + " یا "+ DateTime.Now.ToPersianDate());

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public IActionResult getDate()
        {
            try
            {
                return this.DataFunction(true, DateTime.Now);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public IActionResult getDateString()
        {
            try
            {
                return this.DataFunction(true, DateTime.Now.ToEnglishDate() + " یا "+ DateTime.Now.ToPersianDate());
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }




    }
}