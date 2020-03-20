using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StudentScoreController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IConfiguration _config;

        public StudentScoreController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentScore param)
        {
            try
            {
                param.DateCreate = DateTime.Now;

                await db.StudentScores.AddAsync(param);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] StudentScore param)
        {
            try
            {
                db.Update(param);

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

                var sl = db.StudentScores
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Type = c.Type,
                        Subject = c.Subject,
                        Value = c.Value,
                        studentFullName = c.StdClassMng.Student.Name + " " + c.StdClassMng.Student.LastName,
                        studentId = c.StdClassMng.StudentId
                    })
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Title.Contains(query) || c.Subject.Contains(query) || c.Type.Contains(query)
                            || c.Value.ToString().Contains(query) || c.studentFullName.Contains(query));
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("studentFullName"))
                    {
                        sl = sl.OrderBy(c => c.studentFullName);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderBy(c => c.Type);
                    }
                    if (getparams.sort.Equals("subject"))
                    {
                        sl = sl.OrderBy(c => c.Subject);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("studentFullName"))
                    {
                        sl = sl.OrderByDescending(c => c.studentFullName);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderByDescending(c => c.Type);
                    }
                    if (getparams.sort.Equals("subject"))
                    {
                        sl = sl.OrderByDescending(c => c.Subject);
                    }
                }
                else
                {
                    sl = sl.OrderBy(c => c.Id);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl.ToListAsync();

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

                var sl = await db.StudentScores
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Type = c.Type,
                        Subject = c.Subject,
                        Value = c.Value,
                        sstudentFullName = c.StdClassMng.Student.Name + " " + c.StdClassMng.Student.LastName,
                        studentId = c.StdClassMng.StudentId,
                        StdClassMngId = c.StdClassMngId
                    })
                .ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getStudentScore([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.StudentScores
                        .Select(c => new
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Type = c.Type,
                            Subject = c.Subject,
                            Value = c.Value,
                            studentFullName = c.StdClassMng.Student.Name + " " + c.StdClassMng.Student.LastName,
                            studentId = c.StdClassMng.StudentId,
                            StdClassMngId = c.StdClassMngId,
                            TeacherId = c.TeacherId
                        })
                    .FirstOrDefaultAsync(c => c.Id == id);

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
        public async Task<IActionResult> getAllByStudent_Grade([FromBody] getByStdGradeParam param)
        {
            try
            {
                var stdScore = await db.StudentScores
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Value = c.Value,
                        Type = c.Type,
                        Subject = c.Subject,
                        DateCreate = c.DateCreate,
                        dateCreateString = c.DateCreate.ToPersianDate(),
                        teacherName = c.Teacher.Name,
                        studentId = c.StdClassMng.StudentId,
                        gradeId = c.StdClassMng.GradeId,
                    })
                    .Where(c => c.studentId == param.studentId && c.gradeId == param.gradeId)
                        .OrderByDescending(c => c.DateCreate)
                .ToListAsync();


                return this.DataFunction(true, stdScore);
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

                        var sl = await db.StudentScores
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.StudentScores.Remove(sl);
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