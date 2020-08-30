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
    public class QuestionController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public QuestionController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] QuestionAddParam param)
        {
            try
            {
                param.question.QuestionOptions = param.options.ToList();

                await db.Questions.AddAsync(param.question);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Question question)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var que = await db.Questions.Include(c => c.Grade).SingleAsync(c => c.Id == question.Id);

                if (que.Grade.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("این سوال نمی تواند ویرایش شود");
                }

                que.Name = question.Name;
                que.Type = question.Type;
                que.Title = question.Title;
                que.CourseId = question.CourseId;
                que.GradeId = question.GradeId;
                que.Person = question.Person;
                que.Source = question.Source;
                que.Mark = question.Mark;
                que.Defact = question.Defact;
                que.Answer = question.Answer;
                que.Desc1 = question.Desc1;
                que.Desc2 = question.Desc2;
                que.ComplatabelContent = question.ComplatabelContent;

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

                var que = db.Questions
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Title = c.Title,
                        gradeName = c.Grade.Name,
                        courseName = c.Course.Name,
                        Person = c.Person,
                        markString = c.markString,
                        Type = c.Type,
                        YeareducationId = c.Grade.YeareducationId,
                        Mark = c.Mark,

                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    que = que.Where(c => c.Name.Contains(query) || c.courseName.Contains(query) ||
                                    c.gradeName.Contains(query) || c.Person.Contains(query) ||
                                    c.Title.Contains(query) || c.Mark.ToString().Contains(query));
                }

                count = que.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        que = que.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        que = que.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        que = que.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("gradeName"))
                    {
                        que = que.OrderBy(c => c.gradeName);
                    }
                    if (getparams.sort.Equals("courseName"))
                    {
                        que = que.OrderBy(c => c.courseName);
                    }
                    if (getparams.sort.Equals("person"))
                    {
                        que = que.OrderBy(c => c.Person);
                    }
                    if (getparams.sort.Equals("markString"))
                    {
                        que = que.OrderBy(c => c.Mark);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        que = que.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        que = que.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        que = que.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("gradeName"))
                    {
                        que = que.OrderByDescending(c => c.gradeName);
                    }
                    if (getparams.sort.Equals("courseName"))
                    {
                        que = que.OrderByDescending(c => c.courseName);
                    }
                    if (getparams.sort.Equals("person"))
                    {
                        que = que.OrderByDescending(c => c.Person);
                    }
                    if (getparams.sort.Equals("markString"))
                    {
                        que = que.OrderByDescending(c => c.Mark);
                    }
                }
                else
                {
                    que = que.OrderBy(c => c.Id);
                }

                que = que.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                que = que.Take(getparams.pageSize);

                var q = await que.ToListAsync();

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
                var nowYeareducationId = await this.getActiveYeareducationId();

                var que = await db.Questions
                    .Select(c => new { id = c.Id, name = c.Name, YeareducationId = c.Grade.YeareducationId })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();

                return this.DataFunction(true, que);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getQuestions([FromBody] getQuestionsParams param)
        {
            try
            {
                var questions = db.Questions
                    .Include(c => c.QuestionOptions)
                .AsQueryable();

                var count = 0;

                var pageSize = 12;

                if (param.selectedGrade.HasValue)
                {
                    questions = questions.Where(c => c.GradeId == param.selectedGrade.Value);
                }

                if (param.selectedCourse.HasValue)
                {
                    questions = questions.Where(c => c.CourseId == param.selectedCourse.Value);
                }

                if (!string.IsNullOrEmpty(param.searchText))
                {
                    questions = questions.Where(c => c.Name.Contains(param.searchText) || c.Title.Contains(param.searchText));
                }

                count = await questions.CountAsync();

                questions = questions.OrderByDescending(c => c.Id);

                questions = questions.Skip((param.page - 1) * pageSize);
                questions = questions.Take(pageSize);


                var q = await questions
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        typeString = c.getTypeString(c.Type),
                        Type = c.Type,
                        options = c.QuestionOptions.Select(l => new
                        {
                            Id = l.Id,
                            Title = l.Name
                        }),
                        ComplatabelContent = c.ComplatabelContent,
                        Person = c.Person,
                        DefactString = c.getDefctString(c.Defact)
                    })
                .ToListAsync();

                return Json(new jsondata
                {
                    success = true,
                    data = q,
                    type = count.ToString()
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getOptions([FromBody] int questionId)
        {
            try
            {
                var options = await db.QuestionOptions.Where(c => c.QuestionId == questionId).ToListAsync();


                return this.DataFunction(true, options);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetTrueOption([FromBody] QuestionOption option)
        {
            try
            {
                var isTrue = !option.IsTrue;


                await db.QuestionOptions.Where(c => c.QuestionId == option.QuestionId).ForEachAsync(op =>
                {
                    op.IsTrue = false;
                });


                var questionOption = await db.QuestionOptions.FirstOrDefaultAsync(c => c.Id == option.Id);

                questionOption.IsTrue = isTrue;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getAllPersons()
        {
            try
            {
                var que = await db.Questions.Select(c => c.Person).Distinct().ToArrayAsync();

                return this.DataFunction(true, que);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getQuestion([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var que = await db.Questions.SingleAsync(c => c.Id == id);

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

                        var que = await db.Questions
                            .SingleAsync(c => c.Id == id);

                        if (que == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.Questions.Remove(que);
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

    public class getQuestionsParams
    {
        public string searchText { get; set; }
        public int page { get; set; }
        public int? selectedGrade { get; set; }
        public int? selectedCourse { get; set; }
    }

    public class QuestionAddParam
    {
        public Question question { get; set; }

        public QuestionOption[] options { get; set; }
    }
}