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
    public class CourseController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public CourseController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Course course)
        {
            try
            {
                await db.Courses.AddAsync(course);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Course course)
        {
            try
            {
                var cr = await db.Courses.SingleAsync(c => c.Id == course.Id);

                cr.Name = course.Name;
                cr.GradeId = course.GradeId;
                cr.CourseMix = course.CourseMix;
                cr.Order = course.Order;
                cr.Order2 = course.Order2;
                cr.TeacherId = course.TeacherId;
                cr.TeachTime = course.TeachTime;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] CourseGetParam param)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var cr = db.Courses
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CourseMix = c.CourseMix,
                        TeachTime = c.TeachTime,
                        gradeName = c.Grade.Name,
                        teacherName = c.Teacher.Name,
                        haveTimeSchedules = c.TimeSchedules.Any(),
                        haveExam = c.Exams.Any(),
                        yeareducationId = c.Grade.YeareducationId,
                        GradeId = c.GradeId,
                        TeacherId = c.TeacherId
                    })
                    .Where(c => c.yeareducationId == nowYeareducationId)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    cr = cr.Where(c => c.Name.Contains(query) || c.CourseMix.ToString().Contains(query) ||
                                    c.TeachTime.ToString().Contains(query) || c.gradeName.Contains(query) ||
                                    c.teacherName.ToString().Contains(query));
                }

                
                if (param.selectedGrade.HasValue)
                {
                    cr = cr.Where(c => c.GradeId == param.selectedGrade.Value);
                }

                if (param.selectedTeacher.HasValue)
                {
                    cr = cr.Where(c => c.TeacherId == param.selectedTeacher.Value);
                }

                count = cr.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cr = cr.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        cr = cr.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("courseMix"))
                    {
                        cr = cr.OrderBy(c => c.CourseMix);
                    }
                    if (getparams.sort.Equals("teachTime"))
                    {
                        cr = cr.OrderBy(c => c.TeachTime);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cr = cr.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        cr = cr.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("courseMix"))
                    {
                        cr = cr.OrderByDescending(c => c.CourseMix);
                    }
                    if (getparams.sort.Equals("teachTime"))
                    {
                        cr = cr.OrderByDescending(c => c.TeachTime);
                    }
                }
                else
                {
                    cr = cr.OrderBy(c => c.Id);
                }

                cr = cr.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                cr = cr.Take(getparams.pageSize);

                var q = await cr.ToListAsync();

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

                var cl = await db.Courses
                    .Select(c => new
                    {
                        id = c.Id,
                        name = c.Name + " (" + c.Teacher.Name + ")",
                        TeacherId = c.TeacherId,
                        GradeId = c.GradeId,
                        YeareducationId = c.Grade.YeareducationId,
                        teacherName = c.Teacher.Name
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();

                return this.DataFunction(true, cl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getAllByGrade([FromBody] int gradeId)
        {
            try
            {
                var cl = await db.Courses.Where(c => c.GradeId == gradeId).ToListAsync();

                return this.DataFunction(true, cl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByTeacherGrade([FromBody] getAllByTeacherGradeParam param)
        {
            try
            {
                var courses = await db.Courses
                    .Where(c => c.GradeId == param.gradeId && c.TeacherId == param.teacherId)
                .ToListAsync();

                return this.DataFunction(true, courses);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        [HttpPost]
        public async Task<IActionResult> getCourse([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var cr = await db.Courses.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, cr);
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

                        var cr = await db.Courses.Include(c => c.TimeSchedules)
                            .SingleAsync(c => c.Id == id);

                        if (cr == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (cr.haveTimeSchedules)
                        {
                            return this.UnSuccessFunction(" درس " + cr.Name + " دارای برنامه زمانبندی است", "error");
                        }

                        db.Courses.Remove(cr);
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

    public class CourseGetParam
    {
        public getparams getparams { get; set; }

        public int? selectedGrade { get; set; }
        public int? selectedTeacher { get; set; }
    }

    public class getAllByTeacherGradeParam
    {
        public int teacherId { get; set; }

        public int gradeId { get; set; }
    }
}