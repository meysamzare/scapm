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
    public class QuestionOptionController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public QuestionOptionController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] QuestionOption questionop)
        {
            try
            {
                await db.QuestionOptions.AddAsync(questionop);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] QuestionOption questionop)
        {
            try
            {
                var qop = await db.QuestionOptions.SingleAsync(c => c.Id == questionop.Id);

                qop.Name = questionop.Name;
                qop.Title = questionop.Title;
                qop.IsTrue = questionop.IsTrue;
                qop.QuestionId = questionop.QuestionId;

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

                var qop = db.QuestionOptions
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        IsTrue = c.IsTrue,
                        Name = c.Name,
                        questionName = c.Question.Name,
                        YeareducationId = c.Question.Grade.YeareducationId
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    qop = qop.Where(c => c.Name.Contains(query) || c.questionName.Contains(query));

                }

                count = qop.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        qop = qop.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        qop = qop.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("questionName"))
                    {
                        qop = qop.OrderBy(c => c.questionName);
                    }
                    if (getparams.sort.Equals("istrue"))
                    {
                        qop = qop.OrderBy(c => c.IsTrue);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        qop = qop.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        qop = qop.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("questionName"))
                    {
                        qop = qop.OrderByDescending(c => c.questionName);
                    }
                    if (getparams.sort.Equals("istrue"))
                    {
                        qop = qop.OrderByDescending(c => c.IsTrue);
                    }
                }
                else
                {
                    qop = qop.OrderBy(c => c.Id);
                }

                qop = qop.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                qop = qop.Take(getparams.pageSize);

                var q = await qop.ToListAsync();

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

                var que = await db.QuestionOptions.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, que);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        [HttpPost]
        public async Task<IActionResult> getQuestionOption([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var que = await db.QuestionOptions.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, que);
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

                        var que = await db.QuestionOptions
                            .SingleAsync(c => c.Id == id);

                        if (que == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.QuestionOptions.Remove(que);
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