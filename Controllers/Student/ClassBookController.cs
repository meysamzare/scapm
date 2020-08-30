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
    public class ClassBookController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IConfiguration _config;

        public ClassBookController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClassBook classBook)
        {
            try
            {

                if (classBook.Date < new DateTime(0622, 12, 30))
                {
                    classBook.Date = DateTime.Now;
                }
                else
                {
                    classBook.Date = classBook.Date.AddDays(1);
                }

                if (classBook.Type == ClassBookType.ExamScore)
                {
                    var exam = await db.Exams.FirstOrDefaultAsync(c => c.Id == classBook.ExamId);

                    if (await db.ExamScores.AnyAsync(c => c.ExamId == exam.Id && c.StudentId == classBook.StudentId))
                    {
                        return this.UnSuccessFunction("نمره این دانش آموز قبلا ثبت شده است");
                    }


                    var examScore = new ExamScore
                    {
                        ExamId = exam.Id,
                        Score = double.Parse(classBook.Value),
                        State = ExamScoreState.Hazer,
                        StudentId = classBook.StudentId,
                        TopScore = exam.TopScore,
                        NumberQ = exam.NumberQ,
                        TrueAnswer = 0,
                        FalseAnswer = 0,
                        BlankAnswer = 0
                    };

                    exam.ResultDate = DateTime.Now;
                    exam.Result = true;

                    await db.ExamScores.AddAsync(examScore);
                }

                await db.ClassBooks.AddAsync(classBook);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ClassBook classBook)
        {
            try
            {

                // var clsBook = await db.ClassBooks.FirstOrDefaultAsync(c => c.Id == classBook.Id);

                // var dateBefore = clsBook.Date;

                // if (classBook.Date != dateBefore)
                // {
                //     classBook.Date = classBook.Date.AddDays(1);
                // }

                if (classBook.Type == ClassBookType.ExamScore)
                {
                    var exam = await db.Exams.FirstOrDefaultAsync(c => c.Id == classBook.ExamId);

                    if (await db.ExamScores.AnyAsync(c => c.StudentId == classBook.StudentId && c.ExamId == exam.Id))
                    {
                        var examSC = await db.ExamScores.FirstOrDefaultAsync(c => c.StudentId == classBook.StudentId && c.ExamId == exam.Id);

                        var examScore = new ExamScore
                        {
                            Id = examSC.Id,
                            ExamId = exam.Id,
                            Score = double.Parse(classBook.Value),
                            State = ExamScoreState.Hazer,
                            StudentId = classBook.StudentId,
                            TopScore = exam.TopScore,
                            NumberQ = exam.NumberQ,
                            TrueAnswer = 0,
                            FalseAnswer = 0,
                            BlankAnswer = 0
                        };

                        db.Update(examScore);
                    }
                    else
                    {
                        var examScore = new ExamScore
                        {
                            ExamId = exam.Id,
                            Score = double.Parse(classBook.Value),
                            State = ExamScoreState.Hazer,
                            StudentId = classBook.StudentId,
                            TopScore = exam.TopScore,
                            NumberQ = exam.NumberQ,
                            TrueAnswer = 0,
                            FalseAnswer = 0,
                            BlankAnswer = 0
                        };

                        exam.ResultDate = DateTime.Now;
                        exam.Result = true;

                        await db.ExamScores.AddAsync(examScore);
                    }
                }

                db.Update(classBook);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getClassBookparams param)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.ClassBooks
                .Where(c => c.Grade.YeareducationId == nowYeareducationId)
                    .Include(c => c.Student)
                    .Include(c => c.Grade)
                    .Include(c => c.Class)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Student.Name.Contains(query) || c.Student.LastName.Contains(query));
                }




                if (param.selectedGrade.HasValue)
                {
                    sl = sl.Where(c => c.GradeId == param.selectedGrade.Value);
                }

                if (param.selectedClass.HasValue)
                {
                    sl = sl.Where(c => c.ClassId == param.selectedClass.Value);
                }

                if (param.selectedStudent.HasValue)
                {
                    sl = sl.Where(c => c.StudentId == param.selectedStudent.Value);
                }

                if (param.selectedCourse.HasValue)
                {
                    sl = sl.Where(c => c.CourseId == param.selectedCourse.Value);
                }

                if (param.selectedTeacher.HasValue)
                {
                    sl = sl.Where(c => c.TeacherId == param.selectedTeacher.Value);
                }




                if (param.filtredType == "P_A")
                {
                    sl = sl.Where(c => c.Type == ClassBookType.P_A);
                }

                if (param.filtredType == "ExamScore")
                {
                    sl = sl.Where(c => c.Type == ClassBookType.ExamScore);
                }

                if (param.filtredType == "ClassAsk")
                {
                    sl = sl.Where(c => c.Type == ClassBookType.ClassAsk);
                }

                if (param.filtredType == "Point")
                {
                    sl = sl.Where(c => c.Type == ClassBookType.Point);
                }

                if (param.filtredType == "Discipline")
                {
                    sl = sl.Where(c => c.Type == ClassBookType.Discipline);
                }



                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        sl = sl.OrderBy(c => c.Student.LastName).ThenBy(c => c.Student.Name);
                    }
                    if (getparams.sort.Equals("gardeClass"))
                    {
                        sl = sl.OrderBy(c => c.Grade).ThenBy(c => c.Class);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderBy(c => c.Type);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.Date);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        sl = sl.OrderByDescending(c => c.Student.LastName).ThenByDescending(c => c.Student.Name);
                    }
                    if (getparams.sort.Equals("gardeClass"))
                    {
                        sl = sl.OrderByDescending(c => c.Grade).ThenByDescending(c => c.Class);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderByDescending(c => c.Type);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.Date);
                    }
                }
                else
                {
                    sl = sl.OrderByDescending(c => c.Date);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new
                    {
                        id = c.Id,
                        studentName = c.Student.Name + ' ' + c.Student.LastName,
                        gradeName = c.Grade.Name,
                        className = c.Class.Name,
                        type = c.Type,
                        dateString = c.Date.ToPersianDate(),
                        value = c.Value,
                        ExamName = c.ExamName,
                        teacherName = c.Teacher.Name,
                        courseName = c.Course.Name
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
        public async Task<IActionResult> getClassBook([FromBody] int id)
        {
            try
            {
                if (id != 0)
                {
                    var sl = await db.ClassBooks.FirstOrDefaultAsync(c => c.Id == id);

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
        public async Task<IActionResult> getByStudent([FromBody] getClassBookByStudentParam param)
        {
            try
            {
                var pageSize = 10;

                var classBooks = db.ClassBooks
                    .Where(c => c.StudentId == param.studentId)
                .AsQueryable();

                if (!param.access)
                {
                    classBooks.Where(c => c.TeacherId == param.teacherId);
                }

                var totalCount = await classBooks.CountAsync();

                if (param.sort == "date")
                {
                    classBooks = classBooks.OrderByDescending(c => c.Date);
                }

                if (param.sort == "type")
                {
                    classBooks = classBooks.OrderBy(c => c.Type);
                }

                var lst = await classBooks
                    .Skip((param.page - 1) * pageSize)
                    .Take(pageSize)
                .ToListAsync();


                return this.DataFunction(true, new
                {
                    classBooks = lst,
                    totalCount = totalCount
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByStd_Grade([FromBody] getByStdGradeParam param)
        {
            try
            {
                var classBooks = await db.ClassBooks
                    .Where(c => c.StudentId == param.studentId && c.GradeId == param.gradeId)
                    .Where(c => c.Type != ClassBookType.ExamScore)
                        .Include(c => c.Course)
                        .Include(c => c.Teacher)
                            .OrderByDescending(c => c.Date)
                .ToListAsync();

                return this.DataFunction(true, classBooks);
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

                        var sl = await db.ClassBooks
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        if (sl.Type == ClassBookType.ExamScore)
                        {
                            var score = await db.ExamScores.FirstOrDefaultAsync(c => c.ExamId == sl.ExamId && c.StudentId == sl.StudentId);

                            db.Remove(score);
                        }

                        db.ClassBooks.Remove(sl);
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
    
    public class getByStdGradeParam
    {
        public int studentId { get; set; }

        public int gradeId { get; set; }
    }

    public class getClassBookparams
    {
        public getparams getparams { get; set; }

        public int? selectedGrade { get; set; }
        public int? selectedClass { get; set; }
        public int? selectedStudent { get; set; }
        public int? selectedCourse { get; set; }
        public int? selectedTeacher { get; set; }

        public string filtredType { get; set; }
    }

    public class getClassBookByStudentParam
    {
        public int studentId { get; set; }

        public int teacherId { get; set; }

        public int page { get; set; }

        public string sort { get; set; }

        public bool access { get; set; }
    }
}